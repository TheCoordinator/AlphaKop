using Newtonsoft.Json;

namespace AlphaKop.Core.Captcha.Network {
    public struct CaptchaResponse
    {
        [JsonProperty("captcha")]
        public Captcha Captcha { get; }

        [JsonConstructor]
        public CaptchaResponse(Captcha captcha) {
            Captcha = captcha;
        }
    }
}
