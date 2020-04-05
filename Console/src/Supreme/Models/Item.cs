using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct Item {
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("new_item")]
        public bool? NewItem { get; }

        [JsonProperty("category_name")]
        public string? CategoryName { get; }

        [JsonConstructor]
        public Item(string id, string name, bool? newItem, string? categoryName) {
            Id = id;
            Name = name;
            NewItem = newItem;
            CategoryName = categoryName;
        }

        public override bool Equals(object? obj) {
            if (obj == null) { return false; }
            return ((Item)obj).Id == Id;
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }

        public override string ToString() {
            return
                $"Id: {Id}\n" +
                $"Name: {Name}\n" +
                $"NewItem: {NewItem}\n" +
                $"CategoryName: {CategoryName}";
        }
    }
}
