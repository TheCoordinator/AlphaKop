using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct PookyCheckoutMapping {
        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("mapping")]
        public string? Mapping { get; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string? Value { get; }

        [JsonConstructor]
        public PookyCheckoutMapping(string name, string? mapping, string? value) {
            Name = name;
            Mapping = mapping;
            Value = value;
        }
    }
}
