using System;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaKop.Core.Captcha.Network;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Core.Captcha.Repositories {
    public sealed class CaptchaRepository : ICaptchaRepository {
        private readonly string baseUrl;
        private readonly CaptchaRequestsFactory requestsFactory;
        private readonly HttpClient client;
        private readonly ILogger<CaptchaRepository> logger;

        public CaptchaRepository(
            string baseUrl,
            ILogger<CaptchaRepository> logger
        ) {
            this.baseUrl = baseUrl;
            this.requestsFactory = new CaptchaRequestsFactory(baseUrl);
            this.client = CreateHttpClient(baseUrl);
            this.logger = logger;
        }

        public Task triggerCaptcha(CaptchaRequest request) {
            throw new NotImplementedException();
        }

        public Task cancelTriggerCaptcha(CaptchaRequest request) {
            throw new NotImplementedException();
        }

        public Task<CaptchaResponse> fetchCaptcha() {
            throw new NotImplementedException();
        }

        #region Factory

        private static HttpClient CreateHttpClient(string baseUrl) {
            return new HttpClient() { BaseAddress = new Uri(uriString: baseUrl) };
        }

        #endregion
    }
}