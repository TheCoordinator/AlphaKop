using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AlphaKop.Core.CreditCard;
using AlphaKop.Supreme.Network.Extensions;

namespace AlphaKop.Supreme.Network {
    public interface ISupremeRequestsFactory {
        HttpRequestMessage GetMobileStock();
        HttpRequestMessage GetItemDetails(string itemId);
        HttpRequestMessage AddBasket(AddBasketRequest basketRequest);
        HttpRequestMessage CheckoutTotalsMobile(CheckoutTotalsMobileRequest request);
        HttpRequestMessage Checkout(CheckoutRequest request);
        HttpRequestMessage CheckoutQueue(CheckoutQueueRequest request);
    }

    public sealed class SupremeRequestsFactory : ISupremeRequestsFactory {
        private readonly ICreditCardFormatter creditCardFormatter;

        public SupremeRequestsFactory(
            ICreditCardFormatter creditCardFormatter
        ) {
            this.creditCardFormatter = creditCardFormatter;
        }

        public HttpRequestMessage GetMobileStock() {
            return new HttpRequestMessage {
                RequestUri = new Uri("/mobile_stock.json", UriKind.Relative),
                Method = HttpMethod.Get
            };
        }

        public HttpRequestMessage GetItemDetails(string itemId) => new HttpRequestMessage() {
            RequestUri = new Uri($"/shop/{itemId}.json", UriKind.Relative),
            Method = HttpMethod.Get
        };

        public HttpRequestMessage AddBasket(AddBasketRequest basketRequest) {
            var uri = new Uri($"/shop/{basketRequest.ItemId}/add.json", UriKind.Relative);

            var cookies = new List<IEnumerable<Cookie>>() {
                basketRequest.Pooky.Cookies.StaticCookies,
                basketRequest.Pooky.Cookies.AddToCartCookies
            }.ToCookiesString();

            var message = new HttpRequestMessage() {
                RequestUri = uri,
                Method = HttpMethod.Post
            };

            message.Content = basketRequest.ToFormUrlEncodedContent();

            message.Headers.Add(
                name: HttpRequestHeader.Cookie.ToString(),
                value: cookies
            );

            return message;
        }

        public HttpRequestMessage CheckoutTotalsMobile(CheckoutTotalsMobileRequest request) {
            var queryString = request.GetTotalsMobileJSQueryString();
            var uri = new Uri($"/checkout/totals_mobile.js?{queryString}", UriKind.Relative);
            var cookies = request.Cookies.ToCookiesString();

            var message = new HttpRequestMessage() {
                RequestUri = uri,
                Method = HttpMethod.Get,
            };

            message.Headers.Add(
                name: HttpRequestHeader.Cookie.ToString(),
                value: cookies
            );

            return message;
        }

        public HttpRequestMessage Checkout(CheckoutRequest request) {
            var uri = new Uri("/checkout.json", UriKind.Relative);
            var cookies = request.Cookies.ToCookiesString();

            var message = new HttpRequestMessage() {
                RequestUri = uri,
                Method = HttpMethod.Post
            };

            message.Content = request.ToFormUrlEncodedContent(creditCardFormatter: creditCardFormatter);

            message.Headers.Add(
                name: HttpRequestHeader.Cookie.ToString(),
                value: cookies
            );

            return message;
        }

        public HttpRequestMessage CheckoutQueue(CheckoutQueueRequest request) {
            var uri = new Uri($"/checkout/{request.Slug}/status.json", UriKind.Relative);
            var cookies = request.Cookies.ToCookiesString();

            var message = new HttpRequestMessage() {
                RequestUri = uri,
                Method = HttpMethod.Get
            };

            message.Headers.Add(
                name: HttpRequestHeader.Cookie.ToString(),
                value: cookies
            );

            return message;
        }
    }
}
