using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using AlphaKop.Core.Captcha.Network;
using AlphaKop.Core.Captcha.Repositories;
using AlphaKop.Core.Flows;
using AlphaKop.Core.Network.Http;
using AlphaKop.Supreme.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AlphaKop.Supreme.Flows {
    public interface ICaptchaStep : ITaskStep<CaptchaStepInput> { }

    public sealed class CaptchaStep : ICaptchaStep {
        private const int delayInMilliSeconds = 200;

        private readonly SupremeConfig config;
        private readonly ICaptchaRepository captchaRepository;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public int Retries { get; set; }

        public CaptchaStep(
            IOptions<SupremeConfig> config,
            ICaptchaRepository captchaRepository,
            IServiceProvider provider,
            ILogger<CaptchaStep> logger
        ) {
            this.config = config.Value;
            this.captchaRepository = captchaRepository;
            this.provider = provider;
            this.logger = logger;
        }

        public async Task Execute(CaptchaStepInput input) {
            try {
                await Task.Delay(delayInMilliSeconds);

                var request = CreateCaptchaRequest(input);
                await captchaRepository.TriggerCaptcha(request: request);

                var response = await captchaRepository.FetchCaptcha();

                await PerformPostCaptcha(input, request, response);
            } catch (HttpRequestException ex) {
                LogHttpRequestException(input, ex);

                await RetryStep(input);
            } catch (Exception ex) {
                logger.LogError(input.Job.ToEventId(), ex, "--[Captcha] Unhandled Exception");

                await RetryStep(input);
            }
        }

        private async Task PerformPostCaptcha(CaptchaStepInput input, CaptchaRequest request, CaptchaResponse response) {
            LogResponse(input, response);

            if (response.Captcha.Host == config.SupremeCaptchaHost) {
                await CancelCaptchaTrigger(request);
                if (input.Job.IsCard3DSecureEnabled == true) {
                    await PerformFetchCard3DSecureStep(input, response.Captcha);
                } else {
                    await PerformCheckoutStep(input, response.Captcha);
                }
            } else {
                await RetryStep(input);
            }
        }

        private async Task CancelCaptchaTrigger(CaptchaRequest request) {
            await captchaRepository.CancelTriggerCaptcha(request: request);
        }

        private async Task PerformFetchCard3DSecureStep(CaptchaStepInput input, Captcha captcha) {
            var cardSecureInput = new Card3DSecureStepInput(
                selectedItem: input.SelectedItem,
                pooky: input.Pooky,
                captcha: captcha,
                cookies: input.CheckoutCookies,
                job: input.Job
            );

            await provider.CreateStep<Card3DSecureStepInput, IFetchCard3DSecureStep>()
                .Execute(cardSecureInput);
        }

        private async Task PerformCheckoutStep(CaptchaStepInput input, Captcha captcha) {
            var checkoutStepInput = new CheckoutStepInput(
                selectedItem: input.SelectedItem,
                pooky: input.Pooky,
                captcha: captcha,
                cookies: input.CheckoutCookies,
                job: input.Job
            );

            await provider.CreateStep<CheckoutStepInput, ICheckoutStep>()
                .Execute(checkoutStepInput);
        }

        private CaptchaRequest CreateCaptchaRequest(CaptchaStepInput input) {
            return new CaptchaRequest(
                requestId: input.Job.JobId,
                host: config.SupremeCaptchaHost,
                siteKey: input.Pooky.PageData.SiteKey
            );
        }

        private async Task RetryStep(CaptchaStepInput input) {
            await provider.CreateStep<CaptchaStepInput, ICaptchaStep>(Retries + 1)
                .Execute(input);
        }

        private void LogResponse(CaptchaStepInput input, CaptchaResponse response) {
            logger.LogInformation(
                input.Job.ToEventId(),
                $@"--[CaptchaStep] Token Received {input.SelectedItem.ToString()}"
            );
        }

        private void LogHttpRequestException(CaptchaStepInput input, HttpRequestException ex) {
            if (ex.InnerException != null) {
                if (ex.InnerException is SocketException) {
                    logger.LogError(input.Job.ToEventId(), "--[CaptchaStep] Cannot connect to Captcha Server. Is it running?");
                    return;
                } else if (ex.InnerException is HttpResponseException) {
                    var responseException = (HttpResponseException)ex.InnerException;

                    if (responseException.Response.StatusCode == HttpStatusCode.NotFound) {
                        logger.LogError(input.Job.ToEventId(), "--[CaptchaStep] Captcha Not Found");
                        return;
                    }
                }
            }

            logger.LogError(input.Job.ToEventId(), ex, "--[CaptchaStep] Unhandled HttpRequestException");
        }
    }
}
