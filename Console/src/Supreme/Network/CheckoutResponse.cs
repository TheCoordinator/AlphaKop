using System.Collections.Generic;
using Newtonsoft.Json;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutResponse {
        [JsonProperty("status")]
        public string Status { get; }

        [JsonProperty("slug")]
        public string? slug { get; }

        [JsonProperty("id")]
        public string? Id { get; }

        [JsonProperty("info")]
        public CheckoutResponseInfo? Info { get; }

        [JsonProperty("mpa")]
        public IEnumerable<CheckoutResponsePurchaseAttempt>? PurchaseAttempts { get; }

        [JsonConstructor]
        public CheckoutResponse(
            string status,
            string? slug,
            string? id,
            CheckoutResponseInfo? info,
            IEnumerable<CheckoutResponsePurchaseAttempt>? purchaseAttempts
        ) {
            Status = status;
            this.slug = slug;
            Id = id;
            Info = info;
            PurchaseAttempts = purchaseAttempts;
        }
    }
}
