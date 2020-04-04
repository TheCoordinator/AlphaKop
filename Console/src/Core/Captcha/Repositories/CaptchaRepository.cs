using System;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaKop.Core.Captcha.Config;
using AlphaKop.Core.Captcha.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AlphaKop.Core.Captcha.Repositories {
    public sealed class CaptchaRepository : ICaptchaRepository {
        private readonly string baseUrl;
        private readonly CaptchaRequestsFactory requestsFactory;
        private readonly HttpClient client;
        private readonly ILogger<CaptchaRepository> logger;

        public CaptchaRepository(
            IOptions<CaptchaConfig> config,
            ILogger<CaptchaRepository> logger
        ) {
            this.baseUrl = config.Value.baseUrl;
            this.requestsFactory = new CaptchaRequestsFactory(baseUrl);
            this.client = CreateHttpClient(baseUrl);
            this.logger = logger;
        }

        public async Task TriggerCaptcha(CaptchaRequest request) {
            var messageRequest = requestsFactory.GetTriggerRequest(request: request);
            var response = await client.SendAsync(request: messageRequest);
            response.EnsureSuccessStatusCode();
        }

        public async Task CancelTriggerCaptcha(CaptchaRequest request) {
            var messageRequest = requestsFactory.GetCancelTriggerRequest(request: request);
            var response = await client.SendAsync(request: messageRequest);
            response.EnsureSuccessStatusCode();
        }

        public Task<CaptchaResponse?> FetchCaptcha() {
            throw new NotImplementedException();
        }

        #region Factory

        private static HttpClient CreateHttpClient(string baseUrl) {
            return new HttpClient() { BaseAddress = new Uri(uriString: baseUrl) };
        }

        #endregion
    }
}