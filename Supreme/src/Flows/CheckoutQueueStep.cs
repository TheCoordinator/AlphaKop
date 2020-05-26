using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AlphaKop.Supreme.Flows {
    public interface ICheckoutQueueStep : ITaskStep<CheckoutQueueStepInput> { }

    public sealed class CheckoutQueueStep : ICheckoutQueueStep {
        private readonly ISupremeCheckoutRepository supremeRepository;
        private readonly IPookyRepository pookyRepository;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public int Retries { get; set; }

        public CheckoutQueueStep(
            ISupremeCheckoutRepository supremeRepository,
            IPookyRepository pookyRepository,
            IServiceProvider provider,
            ILogger<CheckoutQueueStep> logger
        ) {
            this.supremeRepository = supremeRepository;
            this.pookyRepository = pookyRepository;
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
                cookies: input.CheckoutCookies.Cookies
            );
        }

        private async Task PerformPostCheckoutResponse(CheckoutQueueStepInput input, CheckoutQueueRequest request, CheckoutResponse response) {
            LogResponse(input, response);

            var status = response.StatusResponse.Status;

            if (status == "paid" || status == "dup") {
                await PerformSuccessStep(input, response);
            } else if (status == "queued") {
                await RetryStep(input, response);
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

        private async Task RetryStep(CheckoutQueueStepInput input, CheckoutResponse response) {

            var checkoutCookies = input.CheckoutCookies;
            var pookyTicket = await FetchPookyTicket(input, response);

            if (pookyTicket != null) {
                var cookies = checkoutCookies.Cookies
                    .Where(cookie => cookie.Name != "_ticket")
                    .ToList();

                cookies.Add(
                    new Cookie(name: "_ticket", value: pookyTicket.Value.Ticket)
                );

                checkoutCookies = new CheckoutCookies(cookies);
            }

            var slug = response.StatusResponse.Slug ?? input.Slug;

            var newInput = new CheckoutQueueStepInput(
                selectedItem: input.SelectedItem,
                checkoutCookies: checkoutCookies,
                slug: slug,
                job: input.Job
            );

            await RetryStep(newInput);
        }

        private async Task RetryStep(CheckoutQueueStepInput input) {
            await provider.CreateStep<CheckoutQueueStepInput, ICheckoutQueueStep>(Retries + 1)
                .Execute(input);
        }

        private async Task PerformPostCheckoutFailed(CheckoutQueueStepInput input, CheckoutResponse response) {
            var purchaseAttempt = response.StatusResponse.PurchaseAttempt;

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

        private async Task<PookyTicket?> FetchPookyTicket(
            CheckoutQueueStepInput input,
            CheckoutResponse response
        ) {
            if (response.Ticket == null) {
                return null;
            }

            return await pookyRepository.FetchPookyTicket(
                region: PookyRegionUtil.From(input.Job.Region),
                ticket: response.Ticket
            );
        }

        private void LogResponse(CheckoutQueueStepInput input, CheckoutResponse response) {
            logger.LogInformation(
                input.Job.ToEventId(),
                $@"--[CheckoutQueue] Status [{response.StatusResponse.Status}] {input.SelectedItem.ToString()}"
            );
        }
    }
}
