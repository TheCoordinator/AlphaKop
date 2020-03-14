using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DaraBot.Supreme.Models;
using DaraBot.Supreme.Requests;
using System.Collections.Generic;
using DaraBot.Supreme.Responses;

namespace DaraBot.Supreme.Repositories
{
    sealed class SupremeRepository : ISupremeRepository
    {
        private readonly string baseUrl;
        private readonly SupremeRequestsFactory requestsFactory;
        private readonly HttpClient client;

        public SupremeRepository(string baseUrl = "https://www.supremenewyork.com")
        {
            this.baseUrl = baseUrl;
            this.requestsFactory = new SupremeRequestsFactory(baseUrl: baseUrl);
            this.client = CreateHttpClient(baseUrl: baseUrl, requestsFactory: requestsFactory);
        }

        public async Task<Stock> FetchStock()
        {
            return await SendJsonRequest<Stock>(
                request: requestsFactory.MobileStock
            );
        }

        public async Task<ItemDetails> FetchItemDetails(string itemId)
        {
            return await SendJsonRequest<ItemDetails>(
                request: requestsFactory.GetItemDetails(itemId: itemId)
            );
        }

        public async Task<IEnumerable<AddBasketResponse>> AddToBasket(AddBasketRequest basketRequest)
        {
            return await SendJsonRequest<List<AddBasketResponse>>(
                request: requestsFactory.AddToBasket(basketRequest: basketRequest)
            );
        }

        private async Task<T> SendJsonRequest<T>(HttpRequestMessage request)
        {
            var response = await client.SendAsync(request: request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        #region Factory

        private static HttpClient CreateHttpClient(string baseUrl, SupremeRequestsFactory requestsFactory)
        {
            var client = new HttpClient() { BaseAddress = new Uri(uriString: baseUrl) };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add(HttpRequestHeader.UserAgent.ToString(), client.GetSupremeMobileUserAgent());

            return client;
        }

        #endregion
    }
}
