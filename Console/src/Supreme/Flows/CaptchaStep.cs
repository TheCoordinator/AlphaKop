using System;
using System.Threading.Tasks;
using AlphaKop.Core.Captcha.Network;
using AlphaKop.Core.Captcha.Repositories;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AlphaKop.Supreme.Flows {
    public interface ICaptchaStep : ITaskStep<CaptchaStepParameter, SupremeJob> { }

    sealed class CaptchaStep : BaseStep<CaptchaStepParameter>, ICaptchaStep {
        private readonly SupremeConfig config;
        private readonly ICaptchaRepository captchaRepository;
        private readonly ILogger logger;

        public CaptchaStep(
            IOptions<SupremeConfig> config,
            ICaptchaRepository captchaRepository,
            IServiceProvider provider,
            ILogger<CaptchaStep> logger
        ) : base(provider) {
            this.config = config.Value;
            this.captchaRepository = captchaRepository;
            this.logger = logger;
        }

        protected override async Task Execute(CaptchaStepParameter parameter, SupremeJob job) {
            try {
                var request = new CaptchaRequest(
                    requestId: job.JobId,
                    host: config.SupremeCaptchaHost,
                    siteKey: parameter.Pooky.PageData.SiteKey
                );

                await captchaRepository.TriggerCaptcha(request: request);

                var response = await captchaRepository.FetchCaptcha();
                var captcha = response.Captcha;

                LogResponse(response, parameter);

                if (captcha.Host == config.SupremeCaptchaHost) {
                    await captchaRepository.CancelTriggerCaptcha(request: request);

                    var checkoutStepParameter = new CheckoutStepParameter(
                        selectedItem: parameter.SelectedItem,
                        basketResponse: parameter.BasketResponse,
                        pooky: parameter.Pooky,
                        pookyTicket: parameter.PookyTicket,
                        captcha: captcha
                    );

                    await provider.CreateCheckoutStep(job, 0)
                        .Execute(checkoutStepParameter);
                } else {
                    await provider.CreateCaptchaStep(job, Retries + 1)
                        .Execute(parameter);
                }
            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "--[Captcha] Error");

                await provider.CreateCaptchaStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }

        private void LogResponse(CaptchaResponse response, CaptchaStepParameter parameter) {
            logger.LogInformation(
                JobEventId, 
                $@"--[Captcha] Token [{response.Captcha.Token}] {parameter.SelectedItem.ToString()}"
            );
        }        
    }
}
