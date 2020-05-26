using System.Net.Http;
using System.Threading.Tasks;

namespace AlphaKop.Core.Network.Http {
    public interface Link {
        HttpRequestMessage CreateRequest();
    }

    public interface Link<TResponse> : Link {
        Task<TResponse> ParseResponseAsync(HttpResponseMessage response);
    }
}