using System;
using System.Net;
using System.Net.Http;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Network {
    public interface IPookyRequestsFactory {
        HttpRequestMessage Pooky(PookyRegion region);
        HttpRequestMessage PookyTicket(PookyRegion region, string ticket);
    }

    public sealed class PookyRequestsFactory : IPookyRequestsFactory {
        public HttpRequestMessage Pooky(PookyRegion region) {
            string regionName = RegionName(region: region);
            return new HttpRequestMessage {
                RequestUri = new Uri($"/{regionName}/all", UriKind.Relative),
                Method = HttpMethod.Get,
            };
        }

        public HttpRequestMessage PookyTicket(PookyRegion region, string ticket) {
            string regionName = RegionName(region: region);
            return new HttpRequestMessage {
                RequestUri = new Uri($"/{regionName}/ticket/{ticket}", UriKind.Relative),
                Method = HttpMethod.Get,
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