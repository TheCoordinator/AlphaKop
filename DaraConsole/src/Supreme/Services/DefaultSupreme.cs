using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DaraBot.Supreme.Entities;

namespace DaraBot.Supreme.Services
{
    sealed class DefaultSupreme : ISupreme
    {
        private const string BASE_URL = "https://www.supremenewyork.com";

        public async Task<Stock> FetchStock()
        {
            using (var client = new HttpClient())
            {
                var request = GetMobileStockRequestMessage();
                var response = await client.SendAsync(request: request);
                response.EnsureSuccessStatusCode();

                string jsonBody = await response.Content.ReadAsStringAsync();
                return Stock.FromJson(json: jsonBody);
            }
        }

        #region URIs
        private HttpRequestMessage GetMobileStockRequestMessage()
        {
            return new HttpRequestMessage
            {
                RequestUri = new Uri(BASE_URL + "/mobile_stock.json"),
                Method = HttpMethod.Get,
                Headers = {
                    { HttpRequestHeader.UserAgent.ToString(), Constants.MOBILE_USER_AGENT }
                }
            };
        }
        #endregion
    }
}