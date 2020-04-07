using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AlphaKop.Supreme.Network.Extensions;

namespace AlphaKop.Supreme.Network {
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

            var cookies = new List<IEnumerable<Cookie>>() {
                basketRequest.Pooky.Cookies.StaticCookies,
                basketRequest.Pooky.Cookies.AddToCartCookies
            }.ToCookiesString();

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

        public HttpRequestMessage Checkout(ICheckoutRequest request) {
            var uriBuilder = new UriBuilder(baseUrl + $"/checkout.json");
            var cookies = GetCheckoutCookies(request);


        }

        private string GetCheckoutCookies(ICheckoutRequest request) {
            return new List<IEnumerable<Cookie>>() {
                request.Pooky.Cookies.StaticCookies,
                request.Pooky.Cookies.CheckoutCookies,
                request.BasketResponse.ResponseCookies,
                new Cookie[] { new Cookie(name: "ticket", value: request.PookyTicket.Ticket) }
            }.ToCookiesString();
        }
    }
}
