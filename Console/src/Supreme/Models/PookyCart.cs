using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct PookyCart {
        [JsonProperty("url")]
        public string Url { get; }

        [JsonProperty("properties")]
        public PookyCartProperty[] Properties { get; }

        [JsonConstructor]
        public PookyCart(string url, PookyCartProperty[] properties) {
            Url = url;
            Properties = properties;
        }
    }
}
