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
                path: $"/checkout/{request.Slug}/status.json",
                request: request,
                responseCookies: request.CheckoutResponse.ResponseCookies
            );
        }

        private HttpRequestMessage CreateCheckoutRequestMessage(string path, ICheckoutRequest request, IEnumerable<Cookie> responseCookies) {
            var uri = new Uri(path, UriKind.Relative);
            var cookies = GetCheckoutCookies(request, responseCookies);

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
