using Newtonsoft.Json;

namespace AlphaKop.Core.Captcha.Network {
    public sealed class CaptchaRequest {
        [JsonProperty("requestId")]
        public string RequestId { get; }

        [JsonProperty("host")]
        public string Host { get; }

        [JsonProperty("siteKey")]
        public string SiteKey { get; }

        public CaptchaRequest(string requestId, string host, string siteKey) {
            RequestId = requestId;
            Host = host;
            SiteKey = siteKey;
        }
    }
}
