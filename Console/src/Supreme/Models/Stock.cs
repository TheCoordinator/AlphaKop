using System.Collections.Generic;
using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct Stock {
        [JsonProperty("products_and_categories")]
        public IDictionary<string, IEnumerable<Item>> Items { get; }

        [JsonProperty("release_week")]
        public string? ReleaseWeek { get; }

        [JsonConstructor]
        public Stock(IDictionary<string, IEnumerable<Item>> items,
            string? releaseWeek) {
            Items = items;
            ReleaseWeek = releaseWeek;
        }
    }
}
