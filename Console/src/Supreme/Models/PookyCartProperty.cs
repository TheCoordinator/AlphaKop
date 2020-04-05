using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct PookyCartProperty {
        [JsonProperty("key")]
        public string Key { get; }

        [JsonProperty("value")]
        public string Value { get; }

        [JsonProperty("literal")]
        public bool Literal { get; }

        [JsonConstructor]
        public PookyCartProperty(string key, string value, bool literal) {
            Key = key;
            Value = value;
            Literal = literal;
        }
    }
}
