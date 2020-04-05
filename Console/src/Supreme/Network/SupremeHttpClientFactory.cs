using System;
using System.Net;
using System.Net.Http;

namespace AlphaKop.Supreme.Network {
    sealed class SupremeHttpClientFactory {
        public static HttpClient CreateHttpClient(string baseUrl) {
            var client = new HttpClient() { BaseAddress = new Uri(uriString: baseUrl) };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add(
                name: HttpRequestHeader.UserAgent.ToString(),
                value: GetSupremeMobileUserAgent()
            );

            client.DefaultRequestHeaders.Add(
                name: HttpRequestHeader.CacheControl.ToString(),
                value: "no-cache"
            );

            return client;
        }

        public static string GetSupremeMobileUserAgent() {
            return "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) CriOS/56.0.2924.75 Mobile/14E5239e Safari/602.1";
        }
    }
}
