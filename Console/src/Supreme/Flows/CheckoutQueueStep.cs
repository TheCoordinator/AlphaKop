using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Repositories;
using AlphaKop.Supreme.Network;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface ICheckoutQueueStep : ITaskStep<CheckoutQueueStepParameter, SupremeJob> { }

    sealed class CheckoutQueueStep : BaseStep<CheckoutQueueStepParameter>, ICheckoutQueueStep {
        private readonly ISupremeRepository supremeRepository;
        private readonly ILogger<CheckoutQueueStep> logger;

        public CheckoutQueueStep(
            ISupremeRepository supremeRepository,
            IServiceProvider provider,
            ILogger<CheckoutQueueStep> logger
        ) : base(provider) {
            this.supremeRepository = supremeRepository;
            this.logger = logger;
        }

        protected override async Task Execute(CheckoutQueueStepParameter parameter, SupremeJob job) {
            try {
            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "Failed to retrieve Checkout Response");

                await provider.CreateCheckoutQueueStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }

        private async Task<CheckoutResponse> PerformCheckoutQueue(CheckoutQueueStepParameter parameter, SupremeJob job) {
            throw new NotImplementedException();
            // var request = new CheckoutRequest(
            //     itemId: parameter.SelectedItem.Item.Id,
            //     sizeId: parameter.SelectedItem.Size.Id,
            //     styleId: parameter.SelectedItem.Style.Id,
            //     quantity: 1,
            //     basketResponse: parameter.BasketResponse,
            //     pooky: parameter.Pooky,
            //     pookyTicket: parameter.PookyTicket,
            //     captcha: parameter.Captcha,
            //     profile: job.Profile
            // );

            // return await supremeRepository.Checkout(request);
        }

        private async Task PerformPostCheckoutResponse(CheckoutResponse response, CheckoutQueueStepParameter parameter, SupremeJob job) {
            LogResponse(response, parameter);

            var status = response.Status.Status;

            if (status == "paid") {
                await PerformPostCheckoutPaid(response, parameter, job);
            } else if (status == "queued" && response.Status.Slug != null) {
                await PerformPostCheckoutQueued(response.Status.Slug, response, parameter, job);
            } else if (status == "failed") {
                await PerformPostCheckoutFailed(response, parameter, job);
            } else {
                // Not Sure What happened here. Could be outOfStock or an unknwon state. Check the logs
                await RevertToItemDetails(parameter.SelectedItem, job);
            }
        }

        private async Task PerformPostCheckoutPaid(CheckoutResponse response, CheckoutQueueStepParameter parameter, SupremeJob job) {
            var successParam = new SuccessStepParameter(
                selectedItem: parameter.SelectedItem,
                checkoutResponse: response
            );

            await provider.CreateSuccessStep(job)
                .Execute(successParam);
        }

        private async Task PerformPostCheckoutQueued(string slug, CheckoutResponse response, CheckoutQueueStepParameter parameter, SupremeJob job) {
            // TODO
            await Task.Delay(1);
        }

        private async Task PerformPostCheckoutFailed(CheckoutResponse response, CheckoutQueueStepParameter parameter, SupremeJob job) {
            var purchaseAttempt = response.Status.PurchaseAttempt;

            if (purchaseAttempt == null || purchaseAttempt.Value.SoldOut == true) {
                await RevertToItemDetails(parameter.SelectedItem, job);
                return;
            }

            await RevertToFetchPooky(parameter, job);
        }

        private async Task RevertToFetchPooky(CheckoutQueueStepParameter parameter, SupremeJob job) {
            var selectedItem = parameter.SelectedItem;

            await provider.CreateFetchPookyStep(job)
                .Execute(selectedItem);
        }

        private async Task RevertToItemDetails(SelectedItemParameter itemParameter, SupremeJob job) {
            var item = itemParameter.Item;

            await provider.CreateFetchItemDetailsStep(job)
                .Execute(item);
        }

        private void LogResponse(CheckoutResponse response, CheckoutQueueStepParameter parameter) {
            var selectedItem = parameter.SelectedItem;
            logger.LogInformation(
                JobEventId, 
                $@"--[CheckoutQueueStep] Status [{response.Status.Status}] {parameter.SelectedItem.ToString()}"
            );
        }
    }
}
