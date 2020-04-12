using System.Net.Http;
using System.Threading.Tasks;
using AlphaKop.Core.Captcha.Network;
using AlphaKop.Core.Network.Http;

namespace AlphaKop.Core.Captcha.Repositories {
    public sealed class CaptchaRepository : ICaptchaRepository {
        private readonly ICaptchaRequestsFactory requestsFactory;
        private readonly HttpClient client;

        public CaptchaRepository(
            IHttpClientFactory clientFactory,
            ICaptchaRequestsFactory requestsFactory
        ) {
            this.requestsFactory = requestsFactory;
            this.client = clientFactory.CreateClient("captcha");
        }

        public async Task TriggerCaptcha(CaptchaRequest request) {
            var messageRequest = requestsFactory.GetTriggerRequest(request: request);
            var response = await client.SendAsync(request: messageRequest);
            await response.EnsureSuccess();
        }

        public async Task CancelTriggerCaptcha(CaptchaRequest request) {
            var messageRequest = requestsFactory.GetCancelTriggerRequest(request: request);
            var response = await client.SendAsync(request: messageRequest);
            await response.EnsureSuccess();
        }

        public async Task<CaptchaResponse> FetchCaptcha() {
            return await client.ReadJsonAsync<CaptchaResponse>(requestsFactory.GetFetchCaptcha());
        }
    }
}