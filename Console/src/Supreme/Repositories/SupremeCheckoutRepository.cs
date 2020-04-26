using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaKop.Core.Network.Http;
using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Repositories {
    sealed class SupremeCheckoutRepository : ISupremeCheckoutRepository {
        private readonly HttpClient client;
        private readonly ISupremeRequestsFactory requestsFactory;

        public SupremeCheckoutRepository(
            IHttpClientFactory clientFactory,
            ISupremeRequestsFactory requestsFactory
        ) {
            this.client = clientFactory.CreateClient("supreme_checkout");
            this.requestsFactory = requestsFactory;
        }

        public async Task<AddBasketResponse> AddBasket(AddBasketRequest basketRequest) {
            var response = await client.SendAsync(
                request: requestsFactory.AddBasket(basketRequest: basketRequest)
            );

            await response.EnsureSuccess();

            var itemSizesStock = await response.Content.ReadJsonAsync<IEnumerable<ItemAddBasketSizeStock>>();
            var ticket = response.GetCookies()
                .FirstOrDefault(cookie => cookie.Name == "ticket")?
                .Value;
                
            return new AddBasketResponse(
                itemSizesStock: itemSizesStock,
                ticket: ticket
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
