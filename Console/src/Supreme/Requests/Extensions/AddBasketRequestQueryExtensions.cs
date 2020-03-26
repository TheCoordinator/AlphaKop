using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;

namespace AlphaKop.Supreme.Requests.Extensions {
    static class AddBasketRequestQueryExtensions {
        public static FormUrlEncodedContent ToFormUrlEncodedContent(this AddBasketRequest request) {
            return new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("size", request.SizeId),
                    new KeyValuePair<string, string>("style", request.StyleId),
                    new KeyValuePair<string, string>("qty", request.Quantity.ToString())
                }
            );
        }
    }
}
