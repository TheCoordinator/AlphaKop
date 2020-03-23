using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct ItemDetails {
        public Item Item { get; internal set; }
        public Style[] Styles { get; internal set; }

        public ItemDetails(Item item, Style[] styles) {
            Item = item;
            Styles = styles;
        }
    }

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
    }

    public struct Size {
        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("id")]
        public string Id { get; internal set; }

        [JsonProperty("stock_level")]
        public int StockLevel { get; internal set; }

        public bool isStockAvailable {
            get { return StockLevel > 0; }
        }

        [JsonConstructor]
        public Size(string name, string id, int stockLevel) {
            Name = name;
            Id = id;
            StockLevel = stockLevel;
        }
    }
}
