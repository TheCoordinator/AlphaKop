using System.Net.Http;
using System.Text;
using System.Web;

namespace AlphaKop.Supreme.Requests.Extensions {
    static class AddBasketRequestQueryExtensions {
        public static string ToQuery(this AddBasketRequest request) {
            var query = HttpUtility.ParseQueryString(
                query: string.Empty,
                encoding: new UTF8Encoding()
            );
            
            query["size"] = request.SizeId;
            query["style"] = request.StyleId;
            query["qty"] = request.Quantity.ToString();

            return query?.ToString() ?? "";
        }
    }
}
