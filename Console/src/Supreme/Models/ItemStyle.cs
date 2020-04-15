using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct ItemStyle {
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("currency")]
        public string? Currency { get; }

        [JsonProperty("image_url")]
        public string? ImageUrl { get; }

        [JsonProperty("image_url_hi")]
        public string? ImageUrlHigh { get; }

        [JsonProperty("sizes")]
        public IEnumerable<ItemSize> Sizes { get; }

        [JsonConstructor]
        public ItemStyle(
            string id,
            string name,
            string? currency,
            string? imageUrl,
            string? imageUrlHigh,
            IEnumerable<ItemSize> sizes
        ) {
            Id = id;
            Name = name;
            Currency = currency;
            ImageUrl = imageUrl;
            ImageUrlHigh = imageUrlHigh;
            Sizes = sizes;
        }

        public override bool Equals(object? obj) {
            if (obj == null) { return false; }
            return ((ItemStyle)obj).Id == Id;
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}
