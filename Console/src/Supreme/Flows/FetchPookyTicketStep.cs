using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchPookyTicketStep : ITaskStep<PookyTicketStepInput> { }

    public sealed class FetchPookyTicketStep : IFetchPookyTicketStep {
        private const int maxRetries = 10;
        private const int delayInMilliSeconds = 200;

        private readonly IPookyRepository pookyRepository;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public int Retries { get; set; }

        public FetchPookyTicketStep(
            IPookyRepository pookyRepository,
            IServiceProvider provider,
            ILogger<FetchPookyTicketStep> logger
        ) {
            this.pookyRepository = pookyRepository;
            this.provider = provider;
            this.logger = logger;
        }

        public async Task Execute(PookyTicketStepInput input) {
            if (Retries >= maxRetries) {
                await RevertToItemDetailsStep(input);
                return;
            }

            try {
                await Task.Delay(delayInMilliSeconds);

                var pookyRegion = PookyRegionUtil.From(input.Job.Region);
                var pookyTicket = await pookyRepository.FetchPookyTicket(
                    region: pookyRegion,
                    ticket: input.BasketTicket
                );

                await PerformPostPookyTicket(input, pookyTicket);
            } catch (Exception ex) {
                logger.LogError(input.Job.ToEventId(), ex, "--[FetchPookyTicketStep] Unhandled Exception");

                await RetryStep(input);
            }
        }

        private async Task PerformPostPookyTicket(PookyTicketStepInput input, PookyTicket pookyTicket) {
            LogResponse(input, pookyTicket);

            await PerformCaptchaStep(input, pookyTicket);
        }

        private async Task PerformCaptchaStep(PookyTicketStepInput input, PookyTicket pookyTicket) {
            var checkoutCookies = new CheckoutCookies(
                new List<IEnumerable<Cookie>>() {
                    input.Pooky.Cookies.StaticCookies,
                    input.Pooky.Cookies.CheckoutCookies,
                    new Cookie[] { new Cookie(name: "_ticket", value: pookyTicket.Ticket) }
                }
            );

            var captchaStepInput = new CaptchaStepInput(
                selectedItem: input.SelectedItem,
                pooky: input.Pooky,
                checkoutCookies: checkoutCookies,
                job: input.Job
            );

            await provider.CreateStep<CaptchaStepInput, ICaptchaStep>()
                .Execute(captchaStepInput);
        }

        private async Task RevertToItemDetailsStep(PookyTicketStepInput input) {
            var itemDetailsInput = new ItemDetailsStepInput(
                item: input.SelectedItem.Item,
                job: input.Job
            );

            await provider.CreateStep<ItemDetailsStepInput, IFetchItemDetailsStep>()
                .Execute(itemDetailsInput);
        }

        private async Task RetryStep(PookyTicketStepInput input) {
            await provider.CreateStep<PookyTicketStepInput, IFetchPookyTicketStep>(Retries + 1)
                .Execute(input);
        }

        private void LogResponse(PookyTicketStepInput input, PookyTicket response) {
            logger.LogInformation(
                input.Job.ToEventId(),
                $@"--[FetchPookyTicketStep] Status [Fetched] {input.SelectedItem.ToString()}"
            );
        }
    }
}
