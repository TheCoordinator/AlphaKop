using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaKop.Core.CreditCard;
using AlphaKop.Core.Network.Http;
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
            ICreditCardFormatter creditCardFormatter,
            ILogger<SupremeRepository> logger
        ) {
            this.baseUrl = config.Value.SupremeBaseUrl;
            this.requestsFactory = new SupremeRequestsFactory(baseUrl: baseUrl, creditCardFormatter: creditCardFormatter);
            this.client = SupremeHttpClientFactory.CreateHttpClient(baseUrl: baseUrl);
            this.logger = logger;
        }

        public async Task<Stock> FetchStock() {
            return await client.ReadJsonAsync<Stock>(
                request: requestsFactory.MobileStock
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

        public async Task<AddBasketResponse> AddBasket(AddBasketRequest basketRequest) {
            var response = await client.SendAsync(
                request: requestsFactory.AddBasket(basketRequest: basketRequest)
            );

            await response.EnsureSuccess();

            var itemSizesStock = await response.Content.ReadJsonAsync<IEnumerable<ItemAddBasketSizeStock>>();
            var cookies = response.GetCookies();

            return new AddBasketResponse(
                itemSizesStock: itemSizesStock,
                responseCookies: cookies
            );
        }

        public async Task<CheckoutResponse> Checkout(ICheckoutRequest request) {
            var response = await client.SendAsync(
                request: requestsFactory.Checkout(request: request)
            );

            await response.EnsureSuccess();

            var status = await response.Content.ReadJsonAsync<CheckoutResponseStatus>();
            var cookies = response.GetCookies();

            return new CheckoutResponse(
                status: status,
                responseCookies: cookies
            );
        }
    }
}
