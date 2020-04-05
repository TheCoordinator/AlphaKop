using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct PookyPageData {
        [JsonProperty("mappings")]
        public PookyCheckoutMapping[] Mappings { get; }

        [JsonProperty("siteKey")]
        public string SiteKey { get; }

        [JsonProperty("region")]
        public string Region { get; }

        [JsonProperty("cart")]
        public PookyCart Cart { get; }

        [JsonConstructor]
        public PookyPageData(
            PookyCheckoutMapping[] mappings,
            string siteKey,
            string region,
            PookyCart cart
        ) {
            Mappings = mappings;
            SiteKey = siteKey;
            Region = region;
            Cart = cart;
        }
    }
}
