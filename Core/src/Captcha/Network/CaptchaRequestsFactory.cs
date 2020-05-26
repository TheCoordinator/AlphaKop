using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace AlphaKop.Core.Captcha.Network {
    public interface ICaptchaRequestsFactory {
        HttpRequestMessage GetFetchCaptcha();
        HttpRequestMessage GetCancelTriggerRequest(CaptchaRequest request);
        HttpRequestMessage GetTriggerRequest(CaptchaRequest request);
    }

    public sealed class CaptchaRequestsFactory : ICaptchaRequestsFactory {
        public HttpRequestMessage GetFetchCaptcha() {
            return new HttpRequestMessage {
                RequestUri = new Uri("/fetch", UriKind.Relative),
                Method = HttpMethod.Get
            };
        }

        public HttpRequestMessage GetTriggerRequest(CaptchaRequest request) {
            var uriBuilder = new UriBuilder();

            var message = new HttpRequestMessage() {
                RequestUri = new Uri("/trigger", UriKind.Relative),
                Method = HttpMethod.Post
            };

            var json = JsonConvert.SerializeObject(request);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return message;
        }

        public HttpRequestMessage GetCancelTriggerRequest(CaptchaRequest request) {
            var uriBuilder = new UriBuilder();

            var message = new HttpRequestMessage() {
                RequestUri = new Uri("/canceltrigger", UriKind.Relative),
                Method = HttpMethod.Post
            };

            var json = JsonConvert.SerializeObject(request);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return message;
        }
    }
}