using System.Net.Http;
using System.Threading.Tasks;
using AlphaKop.Core.Network.Http;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Repositories {
    public sealed class PookyRepository : IPookyRepository {
        private readonly IPookyRequestsFactory requestsFactory;
        private readonly HttpClient client;

        public PookyRepository(
            IHttpClientFactory clientFactory,
            IPookyRequestsFactory requestsFactory
        ) {
            this.requestsFactory = requestsFactory;
            this.client = clientFactory.CreateClient("pooky");
        }

        public async Task<Pooky> FetchPooky(PookyRegion region) {
            return await client.ReadJsonAsync<Pooky>(
                request: requestsFactory.Pooky(region)
            );
        }

        public async Task<Pooky> FetchPooky(PookyItemRequest request) {
            return await client.ReadJsonAsync<Pooky>(
                request: requestsFactory.PookyItem(request: request)
            );
        }

        public async Task<PookyTicket> FetchPookyTicket(PookyRegion region, string ticket) {
            return await client.ReadJsonAsync<PookyTicket>(
                request: requestsFactory.PookyTicket(region: region, ticket: ticket)
            );
        }
    }
}
