using System;
using System.Net;
using System.Net.Http;
using DaraBot.Supreme.Models;

namespace DaraBot.Supreme.Requests {
    sealed class PookyRequestsFactory {
        private readonly string baseUrl;
        private readonly PookyRegion region;
        public string Authentication { get; set; } = "8E91B240AF4A626CDB692DBEEBCAASF34THY45RG45Y6DEFG0BAC9B2BF8964FE";

        public PookyRequestsFactory(string baseUrl, PookyRegion region) {
            this.baseUrl = baseUrl;
            this.region = region;
        }

        public string regionName {
            get {
                switch (region) {
                    case PookyRegion.EU:
                        return "EU";
                    case PookyRegion.US:
                        return "US";
                    default:
                        return "EU";
                }
            }
        }

        public HttpRequestMessage Pooky => new HttpRequestMessage {
            RequestUri = new Uri(baseUrl + $"/{regionName}/all"),
            Method = HttpMethod.Get,
            Headers = {
                { "Auth", Authentication }
            }
        };
    }
}
