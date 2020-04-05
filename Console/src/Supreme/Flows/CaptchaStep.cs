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
        private readonly ILogger<CaptchaStep> logger;

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

                logger.LogInformation(
                    JobEventId,
                    "Captcha Response\n" +
                    $"Token [{captcha.Token}]\n"+
                    $"Host [{captcha.Host}]"
                );

                if (captcha.Host == config.SupremeCaptchaHost) {
                    await captchaRepository.CancelTriggerCaptcha(request: request);

                    // Next Step
                } else {
                    await provider.CreateCaptchaStep(job, Retries + 1)
                        .Execute(parameter);
                }
            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "Failed to retrieve Captcha");

                await provider.CreateCaptchaStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }
    }
}
