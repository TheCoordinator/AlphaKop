using System;
using System.Net;
using System.Net.Http;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Requests {
    sealed class PookyRequestsFactory {
        private readonly string baseUrl;
        private readonly string authentication;

        public PookyRequestsFactory(string baseUrl, string authentication) {
            this.baseUrl = baseUrl;
            this.authentication = authentication;
        }

        public HttpRequestMessage Pooky(PookyRegion region) {
            string regionName = RegionName(region: region);
            return new HttpRequestMessage {
                RequestUri = new Uri(baseUrl + $"/{regionName}/all"),
                Method = HttpMethod.Get,
                Headers = {
                   { "Auth", authentication }
                }
            };
        }

        private string RegionName(PookyRegion region) {
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
}