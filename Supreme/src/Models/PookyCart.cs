using Newtonsoft.Json;
using System.Collections.Generic;

namespace AlphaKop.Supreme.Models {
    public struct PookyCart {
        [JsonProperty("url")]
        public string Url { get; }

        [JsonProperty("properties")]
        public IEnumerable<PookyCartProperty> Properties { get; }

        [JsonConstructor]
        public PookyCart(string url, IEnumerable<PookyCartProperty> properties) {
            Url = url;
            Properties = properties;
        }
    }
}
