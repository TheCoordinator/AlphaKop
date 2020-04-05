using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaKop.Supreme.Config;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlphaKop.Supreme.Repositories {
    sealed class SupremeRepository : ISupremeRepository {
        private readonly string baseUrl;
        private readonly SupremeRequestsFactory requestsFactory;
        private readonly HttpClient client;
        private readonly ILogger<SupremeRepository> logger;

        public SupremeRepository(
            IOptions<SupremeConfig> config,
            ILogger<SupremeRepository> logger
        ) {
            this.baseUrl = config.Value.SupremeBaseUrl;
            this.requestsFactory = new SupremeRequestsFactory(baseUrl: baseUrl);
            this.client = CreateHttpClient(baseUrl: baseUrl);
            this.logger = logger;
        }

        public async Task<Stock> FetchStock() {
            return await SendJsonRequest<Stock>(
                request: requestsFactory.MobileStock
            );
        }

        public async Task<ItemDetails> FetchItemDetails(Item item) {
            var result = await SendJsonRequest(
                request: requestsFactory.GetItemDetails(itemId: item.Id)
            );

            var styles = result.SelectToken("styles")?
                .Children()
                .Select(e => e.ToObject<ItemStyle>()) ?? Array.Empty<ItemStyle>();

            return new ItemDetails(item: item, styles: styles);
        }

        public async Task<IEnumerable<AddBasketResponse>> AddBasket(AddBasketRequest basketRequest) {
            return await SendJsonRequest<List<AddBasketResponse>>(
                request: requestsFactory.AddBasket(basketRequest: basketRequest)
            );
        }

        private async Task<T> SendJsonRequest<T>(HttpRequestMessage request) {
            var response = await client.SendAsync(request: request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        private async Task<JObject> SendJsonRequest(HttpRequestMessage request) {
            var response = await client.SendAsync(request: request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JObject.Parse(jsonString);
        }
        
        #region Factory

        private static HttpClient CreateHttpClient(string baseUrl) {
            var client = new HttpClient() { BaseAddress = new Uri(uriString: baseUrl) };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add(
                name: HttpRequestHeader.UserAgent.ToString(),
                value: client.GetSupremeMobileUserAgent()
            );

            client.DefaultRequestHeaders.Add(
                name: HttpRequestHeader.CacheControl.ToString(),
                value: "no-cache"
            );

            return client;
        }

        #endregion
    }
}
