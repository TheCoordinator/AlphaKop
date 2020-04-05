using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaKop.Core.Network.Extensions;
using AlphaKop.Supreme.Config;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            this.client = SupremeHttpClientFactory.CreateHttpClient(baseUrl: baseUrl);
            this.logger = logger;
        }

        public async Task<Stock> FetchStock() {
            return await client.SendJsonRequest<Stock>(
                request: requestsFactory.MobileStock
            );
        }

        public async Task<ItemDetails> FetchItemDetails(Item item) {
            var result = await client.SendJsonRequest(
                request: requestsFactory.GetItemDetails(itemId: item.Id)
            );

            var styles = result.SelectToken("styles")?
                .Children()
                .Select(e => e.ToObject<ItemStyle>()) ?? Array.Empty<ItemStyle>();

            return new ItemDetails(item: item, styles: styles);
        }

        public async Task<IEnumerable<ItemAddBasketSizeStock>> AddBasket(AddBasketRequest basketRequest) {
            return await client.SendJsonRequest<List<ItemAddBasketSizeStock>>(
                request: requestsFactory.AddBasket(basketRequest: basketRequest)
            );
        }
    }
}
