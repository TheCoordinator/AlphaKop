using Newtonsoft.Json;

namespace AlphaKop.Supreme.Responses {
    public struct AddBasketResponse {
        [JsonProperty("size_id")]
        public string SizeId { get; internal set; }

        [JsonProperty("in_stock")]
        public bool InStock { get; internal set; }

        [JsonConstructor]
        public AddBasketResponse(string sizeId, bool inStock) {
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
