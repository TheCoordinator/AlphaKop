using System;
using Newtonsoft.Json;

namespace AlphaKop.Core.Captcha.Network {
    public struct Captcha {
        [JsonProperty("token")]
        public string Token { get; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; }

        [JsonProperty("host")]
        public string Host { get; }

        [JsonConstructor]
        public Captcha(string token, DateTime timestamp, string host) {
            Token = token;
            Timestamp = timestamp;
            Host = host;
        }
    }
}
