using AlphaKop.Supreme.Models;
using System;
using System.Net.Http;

namespace AlphaKop.Supreme.Network {
    public interface IPookyRequestsFactory {
        HttpRequestMessage Pooky(PookyRegion region);
        HttpRequestMessage PookyItem(PookyItemRequest request);
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

        public HttpRequestMessage PookyItem(PookyItemRequest request) {
            string regionName = RegionName(region: request.Region);
            return new HttpRequestMessage {
                RequestUri = new Uri($"/{regionName}/all/{request.StyleId}/{request.SizeId}", UriKind.Relative),
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