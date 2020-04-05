using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct PookyCheckoutMapping {
        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("mapping")]
        public string MappingMapping { get; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string? Value { get; }

        [JsonConstructor]
        public PookyCheckoutMapping(string name, string mappingMapping, string? value) {
            Name = name;
            MappingMapping = mappingMapping;
            Value = value;
        }
    }
}
