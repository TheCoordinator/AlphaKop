using System;
using Newtonsoft.Json;

namespace AlphaKop.Core.Captcha.Network {
    public sealed class CaptchaResponse {
        public string Token { get; }
        public DateTime Date { get; }
        public string host { get; }

        [JsonConstructor]
        public CaptchaResponse(string token, DateTime date, string host) {
            Token = token;
            Date = date;
            this.host = host;
        }
    }
}
