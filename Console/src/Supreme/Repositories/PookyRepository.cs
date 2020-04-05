using System.Net.Http;
using System.Threading.Tasks;
using AlphaKop.Core.Network.Http;
using AlphaKop.Supreme.Config;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AlphaKop.Supreme.Repositories {
    sealed class PookyRepository : IPookyRepository {
        private readonly string baseUrl;
        private readonly PookyRequestsFactory requestsFactory;
        private readonly HttpClient client;
        private readonly ILogger<PookyRepository> logger;

        public PookyRepository(IOptions<SupremeConfig> config, ILogger<PookyRepository> logger) {
            this.baseUrl = config.Value.PookyBaseUrl;
            this.requestsFactory = new PookyRequestsFactory(
                baseUrl: baseUrl,
                authentication: config.Value.PookyAuthentication
            );
            this.client = SupremeHttpClientFactory.CreateHttpClient(baseUrl: baseUrl);
            this.logger = logger;
        }

        public async Task<Pooky> FetchPooky(PookyRegion region) {
            return await client.ReadJsonAsync<Pooky>(
                request: requestsFactory.Pooky(region)
            );
        }

        public async Task<PookyTicket> FetchPookyTickte(PookyRegion region, string ticket) {
            return await client.ReadJsonAsync<PookyTicket>(
                request: requestsFactory.PookyTicket(region: region, ticket: ticket);
            );
        }
    }
}
