using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace AlphaKop.Core.Captcha.Network {
    sealed class CaptchaRequestsFactory {
        private readonly string baseUrl;

        public CaptchaRequestsFactory(string baseUrl) {
            this.baseUrl = baseUrl;
        }

        public HttpRequestMessage GetTriggerRequest(CaptchaRequest request) {
            var uriBuilder = new UriBuilder();

            var message = new HttpRequestMessage() {
                RequestUri = new Uri(uriString: baseUrl + "/trigger"),
                Method = HttpMethod.Post
            };

            var json = JsonConvert.SerializeObject(request);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return message;
        }

        public HttpRequestMessage GetCancelTriggerRequest(CaptchaRequest request) {
            var uriBuilder = new UriBuilder();

            var message = new HttpRequestMessage() {
                RequestUri = new Uri(uriString: baseUrl + "/canceltrigger"),
                Method = HttpMethod.Post
            };

            var json = JsonConvert.SerializeObject(request);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return message;
        }
    }
}