using Newtonsoft.Json;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutResponsePurchaseAttempt {
        [JsonProperty("Release Date")]
        public string? ReleaseDate { get; }

        [JsonProperty("Release Week")]
        public string? ReleaseWeek { get; }

        [JsonProperty("Total Cart Cost")]
        public string? TotalCartCost { get; }

        [JsonProperty("Currency")]
        public string? Currency { get; }

        [JsonProperty("Shipping City")]
        public string? ShippingCity { get; }

        [JsonProperty("Shipping Country")]
        public string? ShippingCountry { get; }

        [JsonProperty("Category")]
        public string? Category { get; }

        [JsonProperty("Product Name")]
        public string? ProductName { get; }

        [JsonProperty("Product Number")]
        public string? ProductNumber { get; }

        [JsonProperty("Product Color")]
        public string? ProductColor { get; }

        [JsonProperty("Product Size")]
        public string? ProductSize { get; }

        [JsonProperty("Product Cost")]
        public string? ProductCost { get; }

        [JsonProperty("Failure Reason")]
        public string? FailureReason { get; }

        [JsonProperty("Sold Out?")]
        private bool InternalSoldOut { get; }

        [JsonProperty("Success?")]
        public bool Success { get; }

        public bool SoldOut {
            get {
                return InternalSoldOut == true || FailureReason?.ToLower() == "sold out";
            }
        }

        [JsonConstructor]
        public CheckoutResponsePurchaseAttempt(
            string? releaseDate,
            string? releaseWeek,
            string? totalCartCost,
            string? currency,
            string? shippingCity,
            string? shippingCountry,
            string? category,
            bool soldOutInternal,
            string? productName,
            string? productNumber,
            string? productColor,
            string? productSize,
            string? productCost,
            string? failureReason,
            bool success
        ) {
            ReleaseDate = releaseDate;
            ReleaseWeek = releaseWeek;
            TotalCartCost = totalCartCost;
            Currency = currency;
            ShippingCity = shippingCity;
            ShippingCountry = shippingCountry;
            Category = category;
            ProductName = productName;
            ProductNumber = productNumber;
            ProductColor = productColor;
            ProductSize = productSize;
            ProductCost = productCost;
            FailureReason = failureReason;
            InternalSoldOut = soldOutInternal;
            Success = success;
        }
    }
}
