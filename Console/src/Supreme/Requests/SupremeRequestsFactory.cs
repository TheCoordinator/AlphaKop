using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using AlphaKop.Supreme.Requests.Extensions;

namespace AlphaKop.Supreme.Requests {
    sealed class SupremeRequestsFactory {
        private readonly string baseUrl;

        public SupremeRequestsFactory(string baseUrl) {
            this.baseUrl = baseUrl;
        }

        public HttpRequestMessage MobileStock => new HttpRequestMessage {
            RequestUri = new Uri(baseUrl + "/mobile_stock.json"),
            Method = HttpMethod.Get
        };

        public HttpRequestMessage GetItemDetails(string itemId) => new HttpRequestMessage() {
            RequestUri = new Uri(baseUrl + $"/shop/{itemId}.json"),
            Method = HttpMethod.Get
        };

        public HttpRequestMessage AddBasket(AddBasketRequest basketRequest) {
            var uriBuilder = new UriBuilder(baseUrl + $"/shop/{basketRequest.ItemId}/add.json");

            var cookies = basketRequest.Pooky.ToAddToCartCookiesString();

            var message = new HttpRequestMessage() {
                RequestUri = uriBuilder.Uri,
                Method = HttpMethod.Post
            };

            message.Content = basketRequest.ToFormUrlEncodedContent();
            message.Headers.Add(
                name: HttpRequestHeader.Cookie.ToString(), 
                value: cookies
            );

            message.Headers.Add(
                name: HttpRequestHeader.ContentType.ToString(), 
                value: "application/x-www-form-urlencoded"
            );

            return message;
        }
    }
}
