using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct Size {
        [JsonProperty("id")]
        public string Id { get; internal set; }

        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("stock_level")]
        public int StockLevel { get; internal set; }

        public bool isStockAvailable {
            get { return StockLevel > 0; }
        }

        [JsonConstructor]
        public Size(string id, string name, int stockLevel) {
            Id = id;
            Name = name;
            StockLevel = stockLevel;
        }

        public override bool Equals(object? obj) {
            if (obj == null) { return false; }
            return ((Size)obj).Id == Id;
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }

        public override string ToString() {
            return
                $"(Id: {Id}\n" +
                $"Name: {Name}\n" +
                $"StockLevel: {StockLevel}\n" +
                $"isStockAvailable: {isStockAvailable})";
        }
    }
}
