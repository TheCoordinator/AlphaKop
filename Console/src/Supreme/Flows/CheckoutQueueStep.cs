using System;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Repositories;
using AlphaKop.Supreme.Network;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface ICheckoutQueueStep : ITaskStep<CheckoutQueueStepInput> { }

    public sealed class CheckoutQueueStep : ICheckoutQueueStep {
        private readonly ISupremeCheckoutRepository supremeRepository;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public int Retries { get; set; }

        public CheckoutQueueStep(
            ISupremeCheckoutRepository supremeRepository,
            IServiceProvider provider,
            ILogger<CheckoutQueueStep> logger
        ) {
            this.supremeRepository = supremeRepository;
            this.provider = provider;
            this.logger = logger;
        }

        public async Task Execute(CheckoutQueueStepInput input) {
            try {
                await Task.Delay(input.Job.StartDelay);

                var request = CreateRequest(input);
                var response = await supremeRepository.CheckoutQueue(request);

                await PerformPostCheckoutResponse(input, request, response);
            } catch (Exception ex) {
                logger.LogError(input.Job.ToEventId(), ex, "[CheckoutQueueStep] Unhandled Exception");

                await RetryStep(input);
            }
        }

        private CheckoutQueueRequest CreateRequest(CheckoutQueueStepInput input) {
            return new CheckoutQueueRequest(
                slug: input.Slug,
                cookies: input.CheckoutCookies.CookiesList
            );
        }

        private CheckoutQueueStepInput CreateInput(CheckoutQueueStepInput input, CheckoutResponse response) {
            // TODO: Fix this.
            return new CheckoutQueueStepInput(
                selectedItem: input.SelectedItem,
                checkoutCookies: input.CheckoutCookies,
                slug: response.Slug ?? input.Slug,
                job: input.Job
            );
        }

        private async Task PerformPostCheckoutResponse(CheckoutQueueStepInput input, CheckoutQueueRequest request, CheckoutResponse response) {
            LogResponse(input, response);

            var status = response.Status;

            if (status == "paid" || status == "dup") {
                await PerformSuccessStep(input, response);
            } else if (status == "queued") {
                var newInput = CreateInput(input, response);
                await RetryStep(newInput);
            } else if (status == "failed") {
                await PerformPostCheckoutFailed(input, response);
            } else {
                // Not Sure What happened here. Could be outOfStock or an unknwon state. Check the logs
                await RevertToItemDetailsStep(input);
            }
        }

        private async Task PerformSuccessStep(CheckoutQueueStepInput input, CheckoutResponse response) {
            var successInput = new SuccessStepInput(
                selectedItem: input.SelectedItem,
                checkoutResponse: response,
                job: input.Job
            );

            await provider.CreateStep<SuccessStepInput, ISupremeSuccessStep>()
                .Execute(successInput);
        }

        private async Task RetryStep(CheckoutQueueStepInput input) {
            await provider.CreateStep<CheckoutQueueStepInput, ICheckoutQueueStep>(Retries + 1)
                .Execute(input);
        }

        private async Task PerformPostCheckoutFailed(CheckoutQueueStepInput input, CheckoutResponse response) {
            var purchaseAttempt = response.PurchaseAttempt;

            if (purchaseAttempt == null || purchaseAttempt.Value.SoldOut == true) {
                await RevertToItemDetailsStep(input);
                return;
            }

            await RevertToFetchPookyStep(input);
        }

        private async Task RevertToFetchPookyStep(CheckoutQueueStepInput input) {
            var pookyInput = new PookyStepInput(
                selectedItem: input.SelectedItem,
                job: input.Job
            );

            await provider.CreateStep<PookyStepInput, IFetchPookyStep>()
                .Execute(pookyInput);

        }

        private async Task RevertToItemDetailsStep(CheckoutQueueStepInput input) {
            var itemDetailsInput = new ItemDetailsStepInput(
                item: input.SelectedItem.Item,
                job: input.Job
            );

            await provider.CreateStep<ItemDetailsStepInput, IFetchItemDetailsStep>()
                .Execute(itemDetailsInput);
        }

        private void LogResponse(CheckoutQueueStepInput input, CheckoutResponse response) {
            logger.LogInformation(
                input.Job.ToEventId(),
                $@"--[CheckoutQueue] Status [{response.Status}] {input.SelectedItem.ToString()}"
            );
        }
    }
}
