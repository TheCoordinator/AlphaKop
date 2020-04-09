using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutResponseStatus {
        [JsonProperty("status")]
        public string Status { get; }

        [JsonProperty("slug")]
        public string? Slug { get; }

        [JsonProperty("id")]
        public string? Id { get; }

        [JsonProperty("info")]
        public CheckoutResponseInfo? Info { get; }

        [JsonProperty("mpa")]
        public IEnumerable<CheckoutResponsePurchaseAttempt>? PurchaseAttempts { get; }

        [JsonProperty("mps")]
        public CheckoutResponsePurchaseSale? PurchaseSale { get; }

        public CheckoutResponsePurchaseAttempt? PurchaseAttempt {
            get {
                return PurchaseAttempts?.FirstOrDefault();
            }
        }

        [JsonConstructor]
        public CheckoutResponseStatus(
            string status,
            string? slug,
            string? id,
            CheckoutResponseInfo? info,
            IEnumerable<CheckoutResponsePurchaseAttempt>? purchaseAttempts,
            CheckoutResponsePurchaseSale? purchaseSale
        ) {
            Status = status;
            this.Slug = slug;
            Id = id;
            Info = info;
            PurchaseAttempts = purchaseAttempts;
            PurchaseSale = purchaseSale;
        }
    }
}
