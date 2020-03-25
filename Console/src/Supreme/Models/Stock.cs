using System.Collections.Generic;
using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct Stock {
        [JsonProperty("products_and_categories")]
        public Dictionary<string, Item[]> Items { get; internal set; }

        [JsonProperty("release_week")]
        public string? ReleaseWeek { get; internal set; }

        [JsonConstructor]
        public Stock(Dictionary<string, Item[]> items,
            string? releaseWeek) {
            Items = items;
            ReleaseWeek = releaseWeek;
        }
    }

    public struct Item {
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("new_item")]
        public bool? NewItem { get; internal set; }

        [JsonProperty("category_name")]
        public string? CategoryName { get; internal set; }

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
                $"(Id: {Id}\n" +
                $"Name: {Name}\n" +
                $"NewItem: {NewItem}\n" +
                $"CategoryName: {CategoryName})";
        }
    }
}
