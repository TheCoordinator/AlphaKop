using System.Collections.Generic;
using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct PookyCookies {
        [JsonProperty("static")]
        public IDictionary<string, string> StaticCookies { get; }

        [JsonProperty("atc")]
        public IDictionary<string, string> AddToCartCookies { get; }

        [JsonProperty("checkout")]
        public IDictionary<string, string> CheckoutCookies { get; }

        [JsonConstructor]
        public PookyCookies(
            IDictionary<string, string> staticCookies,
            IDictionary<string, string> addToCartCookies,
            IDictionary<string, string> checkoutCookies
        ) {
            StaticCookies = staticCookies;
            AddToCartCookies = addToCartCookies;
            CheckoutCookies = checkoutCookies;
        }
    }
}
