using AlphaKop.Core.Network.Http;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlphaKop.Supreme.Repositories {
    public sealed class SupremeStockRepository : ISupremeStockRepository {
        private readonly HttpClient client;
        private readonly ISupremeRequestsFactory requestsFactory;

        public SupremeStockRepository(
            IHttpClientFactory clientFactory,
            ISupremeRequestsFactory requestsFactory
        ) {
            this.client = clientFactory.CreateClient("supreme_stock");
            this.requestsFactory = requestsFactory;
        }

        public async Task<Stock> FetchStock() {
            return await client.ReadJsonAsync<Stock>(
                request: requestsFactory.GetMobileStock()
            );
        }

        public async Task<ItemDetails> FetchItemDetails(Item item) {
            var response = await client.SendAsync(
                request: requestsFactory.GetItemDetails(itemId: item.Id)
            );
            await response.EnsureSuccess();

            var result = await response.Content.ReadJsonObjectAsync();

            var styles = result.SelectToken("styles")?
                .Children()
                .Select(e => e.ToObject<ItemStyle>()) ?? Array.Empty<ItemStyle>();

            return new ItemDetails(item: item, styles: styles);
        }
    }
}
