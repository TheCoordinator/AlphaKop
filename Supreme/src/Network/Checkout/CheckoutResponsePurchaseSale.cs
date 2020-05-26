using Newtonsoft.Json;
using System.Collections.Generic;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutResponsePurchaseSale {
        [JsonProperty("Release Date")]
        public string? ReleaseDate { get; }

        [JsonProperty("Release Week")]
        public string? ReleaseWeek { get; }

        [JsonProperty("Total Cart Cost")]
        public string? TotalCartCost { get; }

        [JsonProperty("Currency")]
        public string? Currency { get; }

        [JsonProperty("Products")]
        public IEnumerable<PurchasedProduct> Products { get; }

        [JsonConstructor]
        public CheckoutResponsePurchaseSale(
            string? releaseDate,
            string? releaseWeek,
            string? totalCartCost,
            string? currency,
            IEnumerable<PurchasedProduct> products
        ) {
            ReleaseDate = releaseDate;
            ReleaseWeek = releaseWeek;
            TotalCartCost = totalCartCost;
            Currency = currency;
            Products = products;
        }
    }
}
