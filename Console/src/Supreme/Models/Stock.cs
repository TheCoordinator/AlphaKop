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
}
