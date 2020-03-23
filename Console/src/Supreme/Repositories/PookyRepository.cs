using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaKop.Supreme.Config;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Requests;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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
            this.client = CreateHttpClient(baseUrl: baseUrl);
            this.logger = logger;
        }

        public async Task<Pooky> FetchPooky(PookyRegion region) {
            return await SendJsonRequest<Pooky>(request: requestsFactory.Pooky(region));
        }

        private async Task<T> SendJsonRequest<T>(HttpRequestMessage request) {
            var response = await client.SendAsync(request: request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        #region Factory

        private static HttpClient CreateHttpClient(string baseUrl) {
            var client = new HttpClient() { BaseAddress = new Uri(uriString: baseUrl) };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add(
                name: HttpRequestHeader.UserAgent.ToString(),
                value: client.GetSupremeMobileUserAgent()
            );

            return client;
        }

        #endregion
    }
}
