namespace AlphaKop.Core.Captcha.Network {
    public sealed class CaptchaRequest {
        public string RequestId { get; }
        public string Host { get; }
        public string SiteKey { get; }

        public CaptchaRequest(string requestId, string host, string siteKey) {
            RequestId = requestId;
            Host = host;
            SiteKey = siteKey;
        }
    }
}
