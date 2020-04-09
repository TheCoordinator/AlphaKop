using Newtonsoft.Json;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutPurchase {
        [JsonProperty("product_name")]
        public string? ProductName { get; }

        [JsonProperty("style_name")]
        public string? StyleName { get; }

        [JsonProperty("size_name")]
        public string? SizeName { get; }

        [JsonProperty("price")]
        public long Price { get; }

        [JsonProperty("product_id")]
        public string? ProductId { get; }

        [JsonProperty("style_id")]
        public string? StyleId { get; }

        [JsonProperty("quantity")]
        public int Quantity { get; }

        [JsonConstructor]
        public CheckoutPurchase(
            string? productName,
            string? styleName,
            string? sizeName,
            long price,
            string? productId,
            string? styleId,
            int quantity
        ) {
            ProductName = productName;
            StyleName = styleName;
            SizeName = sizeName;
            Price = price;
            ProductId = productId;
            StyleId = styleId;
            Quantity = quantity;
        }
    }
}
