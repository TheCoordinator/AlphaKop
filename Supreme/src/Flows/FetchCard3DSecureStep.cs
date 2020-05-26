using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;
using AlphaKop.Supreme.Repositories;
using AlphaKop.Supreme.Services;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchCard3DSecureStep : ITaskStep<Card3DSecureStepInput> { }

    public sealed class FetchCard3DSecureStep : IFetchCard3DSecureStep {
        private readonly ISupremeCheckoutRepository supremeRepository;
        private readonly IPookyRepository pookyRepository;
        private readonly ICard3DSecureService cardService;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public int Retries { get; set; }

        public FetchCard3DSecureStep(
            ISupremeCheckoutRepository supremeRepository,
            IPookyRepository pookyRepository,
            ICard3DSecureService cardService,
            IServiceProvider provider,
            ILogger<FetchCard3DSecureStep> logger
        ) {
            this.supremeRepository = supremeRepository;
            this.pookyRepository = pookyRepository;
            this.cardService = cardService;
            this.provider = provider;
            this.logger = logger;
        }

        public async Task Execute(Card3DSecureStepInput input) {
            try {
                var totalsMobileResponse = await PerformCheckoutTotalsMobile(input);
                LogTotalsMobileResponse(input, totalsMobileResponse);

                var card3DSecureResponse = await PerformFetchCard3DSecureResponse(totalsMobileResponse);
                LogCard3DSecureResponse(input, card3DSecureResponse);

                var pookyTicket = await PerformFetchPookyTicket(input, totalsMobileResponse);
                LogPookyTicketResponse(input, pookyTicket);

                await PerformCheckoutStep(input, card3DSecureResponse, pookyTicket);
            } catch (Exception ex) {
                logger.LogError(input.Job.ToEventId(), ex, "--[FetchCard3DSecureStep] Unhandled Exception");

                await PerformCheckoutStep(input, null, null);
            }
        }

        private async Task<CheckoutTotalsMobileResponse> PerformCheckoutTotalsMobile(Card3DSecureStepInput input) {
            var request = new CheckoutTotalsMobileRequest(
                sizeId: input.SelectedItem.Size.Id,
                quantity: input.Job.Quantity,
                cookies: input.Cookies.Cookies,
                profile: input.Job.Profile
            );

            return await supremeRepository.FetchCheckoutTotalsMobile(request);
        }

        private async Task<Card3DSecureResponse> PerformFetchCard3DSecureResponse(CheckoutTotalsMobileResponse totalsMobileResponse) {
            return await cardService.FetchCard3DSecure(totalsMobileResponse.HtmlContent);
        }

        private async Task<PookyTicket?> PerformFetchPookyTicket(Card3DSecureStepInput input, CheckoutTotalsMobileResponse totalsMobileResponse) {
            if (totalsMobileResponse.Ticket != null) {
                var pookyRegion = PookyRegionUtil.From(input.Job.Region);
                return await pookyRepository.FetchPookyTicket(pookyRegion, totalsMobileResponse.Ticket);
            }

            return null;
        }

        private async Task PerformCheckoutStep(
            Card3DSecureStepInput input,
            Card3DSecureResponse? response,
            PookyTicket? pookyTicket
        ) {
            var checkoutCookies = input.Cookies;

            if (pookyTicket != null) {
                checkoutCookies = new CheckoutCookies(
                    new List<IEnumerable<Cookie>>() {
                        input.Pooky.Cookies.StaticCookies,
                        input.Pooky.Cookies.CheckoutCookies,
                        new Cookie[] { new Cookie(name: "_ticket", value: pookyTicket.Value.Ticket) }
                    }
                );
            }

            var checkoutInput = new CheckoutStepInput(
                selectedItem: input.SelectedItem,
                pooky: input.Pooky,
                captcha: input.Captcha,
                card3DSecureResponse: response,
                cookies: checkoutCookies,
                job: input.Job
            );

            await provider.CreateStep<CheckoutStepInput, ICheckoutStep>()
                .Execute(checkoutInput);
        }

        private void LogTotalsMobileResponse(Card3DSecureStepInput input, CheckoutTotalsMobileResponse response) {
            logger.LogInformation(
                input.Job.ToEventId(),
                $@"--[FetchCard3DSecureStep] Received Totals Mobile Response {input.SelectedItem.ToString()}"
            );
        }

        private void LogCard3DSecureResponse(Card3DSecureStepInput input, Card3DSecureResponse response) {
            logger.LogInformation(
                input.Job.ToEventId(),
                $@"--[FetchCard3DSecureStep] Received Totals Mobile Response [{response.CardinalId}] {input.SelectedItem.ToString()}"
            );
        }

        private void LogPookyTicketResponse(Card3DSecureStepInput input, PookyTicket? ticket) {
            if (ticket == null) {
                logger.LogInformation(
                    input.Job.ToEventId(),
                    $@"--[FetchCard3DSecureStep] PookyTicket was not received {input.SelectedItem.ToString()}"
                );
            } else {
                logger.LogInformation(
                    input.Job.ToEventId(),
                    $@"--[FetchCard3DSecureStep] Received Pooky Ticket {input.SelectedItem.ToString()}"
                );
            }
        }
    }
}
