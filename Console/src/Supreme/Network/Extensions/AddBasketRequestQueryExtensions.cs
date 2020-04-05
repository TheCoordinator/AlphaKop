using System.Collections.Generic;
using System.Net.Http;

namespace AlphaKop.Supreme.Network.Extensions {
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
