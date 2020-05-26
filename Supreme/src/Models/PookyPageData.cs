using Newtonsoft.Json;
using System.Collections.Generic;

namespace AlphaKop.Supreme.Models {
    public struct PookyPageData {
        [JsonProperty("mappings")]
        public IEnumerable<PookyCheckoutMapping> Mappings { get; }

        [JsonProperty("siteKey")]
        public string SiteKey { get; }

        [JsonProperty("region")]
        public string Region { get; }

        [JsonProperty("cart")]
        public PookyCart Cart { get; }

        [JsonConstructor]
        public PookyPageData(
            IEnumerable<PookyCheckoutMapping> mappings,
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
