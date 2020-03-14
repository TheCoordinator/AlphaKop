using Newtonsoft.Json;

namespace DaraBot.Supreme.Models
{
    public partial struct ItemDetails
    {
        [JsonProperty("styles")]
        public Style[] Styles { get; set; }

        [JsonProperty("can_add_styles")]
        public bool? CanAddStyles { get; set; }

        [JsonProperty("can_buy_multiple")]
        public bool? CanBuyMultiple { get; set; }

        [JsonProperty("cod_blocked")]
        public bool? CodBlocked { get; set; }

        [JsonProperty("canada_blocked")]
        public bool? CanadaBlocked { get; set; }

        [JsonProperty("purchasable_qty")]
        public int? PurchasableQuantity { get; set; }

        [JsonProperty("new_item")]
        public bool? NewItem { get; set; }

        [JsonProperty("non_eu_blocked")]
        public bool? NonEuBlocked { get; set; }

        [JsonConstructor]
        public ItemDetails(Style[] styles,
                           bool? canAddStyles,
                           bool? canBuyMultiple,
                           bool? codBlocked,
                           bool? canadaBlocked,
                           int? purchasableQuantity,
                           bool? newItem,
                           bool? nonEuBlocked)
        {
            Styles = styles;
            CanAddStyles = canAddStyles;
            CanBuyMultiple = canBuyMultiple;
            CodBlocked = codBlocked;
            CanadaBlocked = canadaBlocked;
            PurchasableQuantity = purchasableQuantity;
            NewItem = newItem;
            NonEuBlocked = nonEuBlocked;
        }
    }

    public partial struct Style
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("image_url")]
        public string? ImageUrl { get; set; }

        [JsonProperty("image_url_hi")]
        public string? ImageUrlHigh { get; set; }

        [JsonProperty("sizes")]
        public Size[] Sizes { get; set; }

        [JsonConstructor]
        public Style(string id,
                     string name,
                     string? currency,
                     string? imageUrl,
                     string? imageUrlHigh,
                     Size[] sizes)
        {
            Id = id;
            Name = name;
            Currency = currency;
            ImageUrl = imageUrl;
            ImageUrlHigh = imageUrlHigh;
            Sizes = sizes;
        }
    }

    public partial struct Size
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("stock_level")]
        public int StockLevel { get; set; }

        public bool isStockAvailable { 
            get { return StockLevel > 0; } 
        }

        [JsonConstructor]
        public Size(string name, string id, int stockLevel)
        {
            Name = name;
            Id = id;
            StockLevel = stockLevel;
        }
    }
}