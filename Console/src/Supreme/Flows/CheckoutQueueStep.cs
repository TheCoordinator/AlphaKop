using System;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Repositories;
using AlphaKop.Supreme.Network;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface ICheckoutQueueStep : ITaskStep<CheckoutQueueStepInput> { }

    public sealed class CheckoutQueueStep : ICheckoutQueueStep {
        private readonly ISupremeRepository supremeRepository;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public int Retries { get; set; }

        public CheckoutQueueStep(
            ISupremeRepository supremeRepository,
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
                itemId: input.SelectedItem.Item.Id,
                sizeId: input.SelectedItem.Size.Id,
                styleId: input.SelectedItem.Style.Id,
                quantity: Math.Max(input.Job.Quantity, 1),
                slug: input.Slug,
                basketResponse: input.CheckoutRequest.BasketResponse,
                checkoutResponse: input.CheckoutResponse,
                pooky: input.CheckoutRequest.Pooky,
                pookyTicket: input.CheckoutRequest.PookyTicket,
                captcha: input.CheckoutRequest.Captcha,
                profile: input.CheckoutRequest.Profile
            );
        }

        private CheckoutQueueStepInput CreateInput(CheckoutQueueStepInput input, CheckoutResponse response) {
            return new CheckoutQueueStepInput(
                selectedItem: input.SelectedItem,
                checkoutRequest: input.CheckoutRequest,
                checkoutResponse: input.CheckoutResponse,
                slug: response.Status.Slug ?? input.Slug,
                job: input.Job
            );
        }

        private async Task PerformPostCheckoutResponse(CheckoutQueueStepInput input, CheckoutQueueRequest request, CheckoutResponse response) {
            LogResponse(input, response);

            var status = response.Status.Status;

            if (status == "paid") {
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
            var purchaseAttempt = response.Status.PurchaseAttempt;

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
                $@"--[CheckoutQueue] Status [{response.Status.Status}] {input.SelectedItem.ToString()}"
            );
        }
    }
}
