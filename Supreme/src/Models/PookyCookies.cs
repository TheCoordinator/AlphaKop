using System.Collections.Generic;
using System.Net;
using AlphaKop.Supreme.Network.Converters;
using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct PookyCookies {
        [JsonProperty("static")]
        [JsonConverter(typeof(CookieJsonConverter))]
        public IEnumerable<Cookie> StaticCookies { get; }

        [JsonProperty("atc")]
        [JsonConverter(typeof(CookieJsonConverter))]
        public IEnumerable<Cookie> AddToCartCookies { get; }

        [JsonProperty("checkout")]
        [JsonConverter(typeof(CookieJsonConverter))]
        public IEnumerable<Cookie> CheckoutCookies { get; }

        [JsonConstructor]
        public PookyCookies(
            IEnumerable<Cookie> staticCookies,
            IEnumerable<Cookie> addToCartCookies,
            IEnumerable<Cookie> checkoutCookies
        ) {
            StaticCookies = staticCookies;
            AddToCartCookies = addToCartCookies;
            CheckoutCookies = checkoutCookies;
        }
    }
}
