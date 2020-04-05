using Newtonsoft.Json;

namespace AlphaKop.Supreme.Network {
    public struct AddBasketStockAvailability {
        [JsonProperty("size_id")]
        public string SizeId { get; }

        [JsonProperty("in_stock")]
        public bool InStock { get; }

        [JsonConstructor]
        public AddBasketStockAvailability(string sizeId, bool inStock) {
            SizeId = sizeId;
            InStock = inStock;
        }

        public override string ToString() {
            return
                $"SizeId: {SizeId}\n" +
                $"InStock: {InStock}";
        }        
    }
}
