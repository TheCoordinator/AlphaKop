using System;
using System.Net;
using System.Net.Http;

namespace DaraBot.Supreme.Repositories
{
    sealed class SupremeRequestsFactory
    {
        private readonly string baseUrl;

        public string MobileUserAgent {
            get { return "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) CriOS/56.0.2924.75 Mobile/14E5239e Safari/602.1"; }
        }
        
        public SupremeRequestsFactory(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public HttpRequestMessage MobileStockRequestMessage => new HttpRequestMessage
        {
            RequestUri = new Uri(baseUrl + "/mobile_stock.json"),
            Method = HttpMethod.Get
        };
    }
}