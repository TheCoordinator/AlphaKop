using System.Net.Http;

namespace AlphaKop.Core.Network.Http {
    public static class LinkExtensions {
        public static TResponse ParseResponse<TResponse>(this Link<TResponse> link, HttpResponseMessage response) {
            return link.ParseResponseAsync(response).Result;
        }
    }
}