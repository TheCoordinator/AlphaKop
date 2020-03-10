using System;
using System.Net;
using System.Net.Http;

namespace DaraBot.Supreme.Repositories
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
    }
}