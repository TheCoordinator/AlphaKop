using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Repositories;
using AlphaKop.Supreme.Network;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface ICheckoutStep : ITaskStep<CheckoutStepParameter, SupremeJob> { }

    sealed class CheckoutStep : BaseStep<CheckoutStepParameter>, ICheckoutStep {
        private const int maxRetries = 5;
        private readonly ISupremeRepository supremeRepository;
        private readonly ILogger logger;

        public CheckoutStep(
            ISupremeRepository supremeRepository,
            IServiceProvider provider,
            ILogger<CheckoutStep> logger
        ) : base(provider) {
            this.supremeRepository = supremeRepository;
            this.logger = logger;
        }

        protected override async Task Execute(CheckoutStepParameter parameter, SupremeJob job) {
            try {
                if (Retries >= maxRetries) {
                    await RevertToItemDetailsStep(parameter.SelectedItem, job);
                    return;
                }

                var request = CreateCheckoutRequest(parameter, job);
                var response = await PerformCheckout(request);
                await PerformPostCheckoutResponse(
                    request: request,
                    response: response,
                    parameter: parameter,
                    job: job
                );
            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "--[Checkout] Error");

                await provider.CreateCheckoutStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }

        private CheckoutRequest CreateCheckoutRequest(CheckoutStepParameter parameter, SupremeJob job) {
            return new CheckoutRequest(
                itemId: parameter.SelectedItem.Item.Id,
                sizeId: parameter.SelectedItem.Size.Id,
                styleId: parameter.SelectedItem.Style.Id,
                quantity: 1,
                basketResponse: parameter.BasketResponse,
                pooky: parameter.Pooky,
                pookyTicket: parameter.PookyTicket,
                captcha: parameter.Captcha,
                profile: job.Profile
            );
        }

        private async Task<CheckoutResponse> PerformCheckout(CheckoutRequest request) {
            return await supremeRepository.Checkout(request);
        }

        private async Task PerformPostCheckoutResponse(
            CheckoutRequest request,
            CheckoutResponse response,
            CheckoutStepParameter parameter,
            SupremeJob job
        ) {
            LogResponse(response, parameter);

            var status = response.Status.Status;

            if (status == "paid") {
                await PerformPostCheckoutPaid(response, parameter, job);
            } else if (status == "queued" && response.Status.Slug != null) {
                await PerformPostCheckoutQueued(request, response, parameter, job);
            } else if (status == "failed") {
                await PerformPostCheckoutFailed(response, parameter, job);
            } else {
                // Not Sure What happened here. Could be outOfStock or an unknwon state. Check the logs
                await RevertToItemDetailsStep(parameter.SelectedItem, job);
            }
        }

        private async Task PerformPostCheckoutPaid(CheckoutResponse response, CheckoutStepParameter parameter, SupremeJob job) {
            var successParam = new SuccessStepParameter(
                selectedItem: parameter.SelectedItem,
                checkoutResponse: response
            );

            await provider.CreateSuccessStep(job)
                .Execute(successParam);
        }

        private async Task PerformPostCheckoutQueued(
            CheckoutRequest request,
            CheckoutResponse response,
            CheckoutStepParameter parameter,
            SupremeJob job
        ) {
            var checkoutQueueParam = new CheckoutQueueStepParameter(
                selectedItem: parameter.SelectedItem,
                checkoutRequest: request,
                checkoutResponse: response
            );

            await provider.CreateCheckoutQueueStep(job)
                .Execute(checkoutQueueParam);
        }

        private async Task PerformPostCheckoutFailed(CheckoutResponse response, CheckoutStepParameter parameter, SupremeJob job) {
            var purchaseAttempt = response.Status.PurchaseAttempt;

            if (purchaseAttempt == null || purchaseAttempt.Value.SoldOut == true) {
                await RevertToItemDetailsStep(parameter.SelectedItem, job);
                return;
            }

            await RevertToFetchPookyStep(parameter, job);
        }

        private async Task RevertToFetchPookyStep(CheckoutStepParameter parameter, SupremeJob job) {
            var pookyInput = new PookyStepInput(
                selectedItem: parameter.SelectedItem,
                job: job
            );

            await provider.CreateStep<PookyStepInput, IFetchPookyStep>()
                .Execute(pookyInput);
        }

        private async Task RevertToItemDetailsStep(SelectedItem itemParameter, SupremeJob job) {
            var itemDetailsInput = new ItemDetailsStepInput(
                item: itemParameter.Item,
                job: job
            );

            await provider.CreateStep<ItemDetailsStepInput, IFetchItemDetailsStep>()
                .Execute(itemDetailsInput);
        }

        private void LogResponse(CheckoutResponse response, CheckoutStepParameter parameter) {
            var selectedItem = parameter.SelectedItem;
            logger.LogInformation(
                JobEventId,
                $@"--[CheckoutStep] Status [{response.Status.Status}] {parameter.SelectedItem.ToString()}"
            );
        }
    }
}
