using Newtonsoft.Json;

namespace AlphaKop.Supreme.Network {
    public struct PurchasedProduct {
        [JsonProperty("Name")]
        public string? Name { get; }

        [JsonProperty("Color")]
        public string? Color { get; }

        [JsonProperty("Size")]
        public string? Size { get; }

        [JsonConstructor]
        public PurchasedProduct(string? name, string? color, string? size) {
            Name = name;
            Color = color;
            Size = size;
        }
    }
}
