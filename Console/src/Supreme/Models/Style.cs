using System.Linq;
using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct Style {
        [JsonProperty("id")]
        public string Id { get; internal set; }

        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("currency")]
        public string? Currency { get; internal set; }

        [JsonProperty("image_url")]
        public string? ImageUrl { get; internal set; }

        [JsonProperty("image_url_hi")]
        public string? ImageUrlHigh { get; internal set; }

        [JsonProperty("sizes")]
        public Size[] Sizes { get; internal set; }

        [JsonConstructor]
        public Style(
            string id,
            string name,
            string? currency,
            string? imageUrl,
            string? imageUrlHigh,
            Size[] sizes
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
            return ((Style)obj).Id == Id;
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }

        public override string ToString() {
            var sizes = string.Join("\n\n", Sizes.Select(s => s.ToString()));

            return
                $"Id: {Id}\n" +
                $"Name: {Name}\n" +
                $"Sizes: \n{sizes}";
        }
    }
}
