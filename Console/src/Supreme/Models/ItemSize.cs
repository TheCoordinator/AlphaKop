using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct ItemSize {
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("stock_level")]
        public int StockLevel { get; }

        public bool isStockAvailable {
            get { return StockLevel > 0; }
        }

        [JsonConstructor]
        public ItemSize(string id, string name, int stockLevel) {
            Id = id;
            Name = name;
            StockLevel = stockLevel;
        }

        public override bool Equals(object? obj) {
            if (obj == null) { return false; }
            return ((ItemSize)obj).Id == Id;
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}
