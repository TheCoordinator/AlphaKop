using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AlphaKop.Core.CreditCard;
using AlphaKop.Supreme.Network.Extensions;

namespace AlphaKop.Supreme.Network {
    sealed class SupremeRequestsFactory {
        private readonly string baseUrl;
        private readonly ICreditCardFormatter creditCardFormatter;

        public SupremeRequestsFactory(
            string baseUrl,
            ICreditCardFormatter creditCardFormatter
        ) {
            this.baseUrl = baseUrl;
            this.creditCardFormatter = creditCardFormatter;
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

        public HttpRequestMessage Checkout(CheckoutRequest request) {
            return CreateCheckoutRequestMessage(
                path: "/checkout.json",
                request: request,
                responseCookies: Array.Empty<Cookie>()
            );
        }

        public HttpRequestMessage CheckoutQueue(CheckoutQueueRequest request) {
            return CreateCheckoutRequestMessage(
                path: $"/checkout/{request.CheckoutResponse.Status.Slug}/status.json",
                request: request,
                responseCookies: request.CheckoutResponse.ResponseCookies
            );
        }

        private HttpRequestMessage CreateCheckoutRequestMessage(string path, ICheckoutRequest request, IEnumerable<Cookie> responseCookies) {
            var uriBuilder = new UriBuilder(baseUrl + path);
            var cookies = GetCheckoutCookies(request, responseCookies);

            var message = new HttpRequestMessage() {
                RequestUri = uriBuilder.Uri,
                Method = HttpMethod.Post
            };

            message.Content = request.ToFormUrlEncodedContent(creditCardFormatter: creditCardFormatter);

            message.Headers.Add(
                name: HttpRequestHeader.Cookie.ToString(),
                value: cookies
            );

            return message;
        }

        private string GetCheckoutCookies(ICheckoutRequest request, IEnumerable<Cookie> responseCookies) {
            return new List<IEnumerable<Cookie>>() {
                request.Pooky.Cookies.StaticCookies,
                request.Pooky.Cookies.CheckoutCookies,
                request.BasketResponse.ResponseCookies,
                responseCookies,
                new Cookie[] { new Cookie(name: "ticket", value: request.PookyTicket.Ticket) }
            }.ToCookiesString();
        }
    }
}
