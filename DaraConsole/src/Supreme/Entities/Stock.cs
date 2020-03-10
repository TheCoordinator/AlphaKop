using System.Collections.Generic;
using Newtonsoft.Json;

namespace DaraBot.Supreme.Entities
{
    public partial struct Stock
    {
        [JsonProperty("products_and_categories")]
        public Dictionary<string, Item[]> Items { get; internal set; }

        [JsonProperty("release_week")]
        public string? ReleaseWeek { get; internal set; }

        [JsonConstructor]
        public Stock(Dictionary<string, Item[]> items,
                     string? releaseWeek)
        {
            Items = items;
            ReleaseWeek = releaseWeek;
        }
    }

    public partial struct Item
    {
        [JsonProperty("id")]
        public long Id { get; }

        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("new_item")]
        public bool? NewItem { get; internal set; }

        [JsonProperty("category_name")]
        public string? CategoryName { get; internal set; }

        [JsonConstructor]
        public Item(long id, string name, bool? newItem, string? categoryName)
        {
            Id = id;
            Name = name;
            NewItem = newItem;
            CategoryName = categoryName;
        }
    }

    public partial struct Stock
    {
        public static Stock FromJson(string json) => JsonConvert.DeserializeObject<Stock>(json);
    }
}