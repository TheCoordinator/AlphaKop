namespace AlphaKop.Core.Captcha.Network {
    sealed class CaptchaRequestsFactory {
        private readonly string baseUrl;

        public CaptchaRequestsFactory(string baseUrl) {
            this.baseUrl = baseUrl;
        }
    }
}