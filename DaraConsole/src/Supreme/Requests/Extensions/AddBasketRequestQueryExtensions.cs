using System.Net.Http;
using System.Web;

namespace DaraBot.Supreme.Requests.Extensions
{
    static class AddBasketRequestQueryExtensions
    {
        public static string ToQuery(this AddBasketRequest request)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["size"] = request.SizeId;
            query["style"] = request.StyleId;
            query["qty"] = request.Quantity.ToString();
            
            return query?.ToString() ?? "";
        }
    }
}