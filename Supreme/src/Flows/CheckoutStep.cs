using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Network;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AlphaKop.Supreme.Flows {
    public interface ICheckoutStep : ITaskStep<CheckoutStepInput> { }

    public sealed class CheckoutStep : ICheckoutStep {
        private const int maxRetries = 3;

        private readonly ISupremeCheckoutRepository supremeRepository;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public int Retries { get; set; }

        public CheckoutStep(
            ISupremeCheckoutRepository supremeRepository,
            IServiceProvider provider,
            ILogger<CheckoutStep> logger
        ) {
            this.supremeRepository = supremeRepository;
            this.provider = provider;
            this.logger = logger;
        }

        public async Task Execute(CheckoutStepInput input) {
            try {
                if (Retries >= maxRetries) {
                    await RevertToItemDetailsStep(input);
                    return;
                }

                await Task.Delay(2500); // TODO: Add this to Job

                var request = CreateCheckoutRequest(input);
                var response = await supremeRepository.Checkout(request);

                await PerformPostCheckoutResponse(
                    input: input,
                    request: request,
                    response: response
                );
            } catch (Exception ex) {
                logger.LogError(input.Job.ToEventId(), ex, "--[Checkout] Unhandled Exception");

                await RetryStep(input);
            }
        }

        private CheckoutRequest CreateCheckoutRequest(CheckoutStepInput input) {
            return new CheckoutRequest(
                itemId: input.SelectedItem.Item.Id,
                sizeId: input.SelectedItem.Size.Id,
                styleId: input.SelectedItem.Style.Id,
                quantity: input.Job.Quantity,
                cookies: input.Cookies.Cookies,
                pooky: input.Pooky,
                captcha: input.Captcha,
                cardinalId: input.Card3DSecureResponse?.CardinalId,
                profile: input.Job.Profile
            );
        }

        private async Task PerformPostCheckoutResponse(
            CheckoutStepInput input,
            CheckoutRequest request,
            CheckoutResponse response
        ) {
            LogResponse(input, response);

            var status = response.StatusResponse.Status;

            if (status == "paid" || status == "dup") {
                await PerformSuccessStep(input, response);
            } else if (status == "queued" && response.StatusResponse.Slug != null) {
                await PerformCheckoutQueuedStep(input, request, response, response.StatusResponse.Slug);
            } else if (status == "failed") {
                await PerformPostCheckoutFailed(input, response);
            } else {
                // Not Sure What happened here. Could be outOfStock or an unknwon state. Check the logs
                await RevertToItemDetailsStep(input);
            }
        }

        private async Task PerformSuccessStep(CheckoutStepInput input, CheckoutResponse response) {
            var successInput = new SuccessStepInput(
                selectedItem: input.SelectedItem,
                checkoutResponse: response,
                job: input.Job
            );

            await provider.CreateStep<SuccessStepInput, ISupremeSuccessStep>()
                .Execute(successInput);
        }

        private async Task PerformCheckoutQueuedStep(
            CheckoutStepInput input,
            CheckoutRequest request,
            CheckoutResponse response,
            string slug
        ) {
            var checkoutQueueInput = new CheckoutQueueStepInput(
                selectedItem: input.SelectedItem,
                checkoutCookies: input.Cookies,
                slug: slug,
                job: input.Job
            );

            await provider.CreateStep<CheckoutQueueStepInput, ICheckoutQueueStep>()
                .Execute(checkoutQueueInput);
        }

        private async Task PerformPostCheckoutFailed(CheckoutStepInput input, CheckoutResponse response) {
            var purchaseAttempt = response.StatusResponse.PurchaseAttempt;

            if (purchaseAttempt == null || purchaseAttempt.Value.SoldOut == true || input.Job.FastMode == true) {
                await RevertToItemDetailsStep(input);
                return;
            }

            await RevertToFetchPookyStep(input);
        }

        private async Task RevertToFetchPookyStep(CheckoutStepInput input) {
            var pookyInput = new PookyStepInput(
                selectedItem: input.SelectedItem,
                job: input.Job
            );

            await provider.CreateStep<PookyStepInput, IFetchPookyStep>()
                .Execute(pookyInput);
        }

        private async Task RevertToItemDetailsStep(CheckoutStepInput input) {
            var itemDetailsInput = new ItemDetailsStepInput(
                item: input.SelectedItem.Item,
                job: input.Job
            );

            await provider.CreateStep<ItemDetailsStepInput, IFetchItemDetailsStep>()
                .Execute(itemDetailsInput);
        }

        private async Task RetryStep(CheckoutStepInput input) {
            await provider.CreateStep<CheckoutStepInput, ICheckoutStep>(Retries + 1)
                .Execute(input);
        }

        private void LogResponse(CheckoutStepInput input, CheckoutResponse response) {
            logger.LogInformation(
                input.Job.ToEventId(),
                $@"--[CheckoutStep] Status [{response.StatusResponse.Status}] {input.SelectedItem.ToString()}"
            );
        }
    }
}
