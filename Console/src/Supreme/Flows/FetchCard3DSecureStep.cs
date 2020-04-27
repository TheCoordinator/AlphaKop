using System;
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
        private const int maxRetries = 5;

        private readonly ISupremeCheckoutRepository supremeRepository;
        private readonly ICard3DSecureService cardService;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public int Retries { get; set; }

        public FetchCard3DSecureStep(
            ISupremeCheckoutRepository supremeRepository,
            ICard3DSecureService cardService,
            IServiceProvider provider,
            ILogger<FetchCard3DSecureStep> logger
        ) {
            this.supremeRepository = supremeRepository;
            this.cardService = cardService;
            this.provider = provider;
            this.logger = logger;
        }

        public async Task Execute(Card3DSecureStepInput input) {
            if (Retries >= maxRetries) {
                // TODO: PerformCheckout
                return;
            }

            try {
                var request = new CheckoutTotalsMobileRequest(
                    sizeId: input.SelectedItem.Size.Id,
                    quantity: input.Job.Quantity,
                    cookies: input.Cookies.CookiesList,
                    profile: input.Job.Profile
                );

                var response = await supremeRepository.FetchCheckoutTotalsMobile(request);
                var cardContent = await cardService.FetchCardinalId(response.HtmlContent);
                logger.LogDebug(input.Job.ToEventId(), "--[FetchCard3DSecureStep] response");
            } catch (Exception ex) {
                logger.LogError(input.Job.ToEventId(), ex, "--[FetchCard3DSecureStep] Unhandled Exception");

                await RetryStep(input);
            }
        }

        private async Task PerformCheckoutStep(Card3DSecureStepInput input, Pooky pooky) {
            // var checkout
            // await provider.CreateStep<CaptchaStepInput, ICaptchaStep>()
            //     .Execute(captchaInput);
        }

        private async Task RetryStep(Card3DSecureStepInput input) {
            await provider.CreateStep<Card3DSecureStepInput, IFetchCard3DSecureStep>(Retries + 1)
                .Execute(input);
        }

        private void LogResponse(PookyStepInput input, Pooky pooky) {
            logger.LogInformation(
                input.Job.ToEventId(),
                $@"--[FetchPookyStep] Status [Fetched] {input.SelectedItem.ToString()}"
            );
        }
    }
}
