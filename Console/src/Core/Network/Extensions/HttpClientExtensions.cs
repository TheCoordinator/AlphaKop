using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlphaKop.Core.Network.Extensions {
    public static class HttpClientExtensions {
        public static async Task<T> SendJsonRequest<T>(
            this HttpClient client,
            HttpRequestMessage request
        ) {
            var response = await client.SendAsync(request: request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static async Task<JObject> SendJsonRequest(
            this HttpClient client,
            HttpRequestMessage request
        ) {
            var response = await client.SendAsync(request: request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JObject.Parse(jsonString);
        }
    }
}