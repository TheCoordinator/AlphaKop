using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DaraBot.Supreme.Models;
using DaraBot.Supreme.Requests;
using Newtonsoft.Json;

namespace DaraBot.Supreme.Repositories {
    sealed class PookyRepository : IPookyRepository {
        private readonly string baseUrl;
        private readonly PookyRegion region;
        private readonly PookyRequestsFactory requestsFactory;
        private readonly HttpClient client;

        public PookyRepository(
            string baseUrl = "https://pooky.speseo.com",
            PookyRegion region = PookyRegion.EU
        ) {
            this.baseUrl = baseUrl;
            this.region = region;
            this.requestsFactory = new PookyRequestsFactory(baseUrl: baseUrl, region: region);
            this.client = CreateHttpClient(baseUrl: baseUrl);
        }

        public async Task<Pooky> FetchPooky() {
            return await SendJsonRequest<Pooky>(request: requestsFactory.Pooky);
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
