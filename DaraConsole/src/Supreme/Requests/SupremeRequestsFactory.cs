using System;
using System.Net.Http;

namespace DaraBot.Supreme.Requests
{
    sealed class SupremeRequestsFactory
    {
        private readonly string baseUrl;

        public SupremeRequestsFactory(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public HttpRequestMessage MobileStock => new HttpRequestMessage
        {
            RequestUri = new Uri(baseUrl + "/mobile_stock.json"),
            Method = HttpMethod.Get
        };

        public HttpRequestMessage GetItemDetails(string itemId) => new HttpRequestMessage()
        {
            RequestUri = new Uri(baseUrl + $"/shop/{itemId}.json"),
            Method = HttpMethod.Get
        };
    }
}
