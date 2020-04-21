using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaKop.Core.Network.Http;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Repositories {
    sealed class SupremeRepository : ISupremeRepository {
        private readonly HttpClient client;
        private readonly ISupremeRequestsFactory requestsFactory;

        public SupremeRepository(
            IHttpClientFactory clientFactory,
            ISupremeRequestsFactory requestsFactory
        ) {
            this.client = clientFactory.CreateClient("supreme");
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

        public async Task<CheckoutResponse> Checkout(CheckoutRequest request) {
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

        public async Task<CheckoutResponse> CheckoutQueue(CheckoutQueueRequest request) {
            var response = await client.SendAsync(
                request: requestsFactory.CheckoutQueue(request: request)
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
