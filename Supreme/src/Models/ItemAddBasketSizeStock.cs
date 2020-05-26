using Newtonsoft.Json;

namespace AlphaKop.Supreme.Network {
    public struct ItemAddBasketSizeStock {
        [JsonProperty("size_id")]
        public string SizeId { get; }

        [JsonProperty("in_stock")]
        public bool InStock { get; }

        [JsonConstructor]
        public ItemAddBasketSizeStock(string sizeId, bool inStock) {
            SizeId = sizeId;
            InStock = inStock;
        }
    }
}
