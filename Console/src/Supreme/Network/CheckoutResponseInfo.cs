using System.Collections.Generic;
using Newtonsoft.Json;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutResponseInfo {
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("billing_name")]
        public string BillingName { get; }

        [JsonProperty("email")]
        public string Email { get; }

        [JsonProperty("purchases")]
        public IEnumerable<CheckoutPurchase> Purchases { get; }

        [JsonProperty("item_total")]
        public long ItemTotal { get; }

        [JsonProperty("shipping_total")]
        public long ShippingTotal { get; }

        [JsonProperty("total")]
        public long Total { get; }

        [JsonProperty("currency")]
        public string? Currency { get; }

        [JsonProperty("created_at")]
        public string? CreatedAt { get; }

        [JsonConstructor]
        public CheckoutResponseInfo(
            string id,
            string billingName,
            string email,
            IEnumerable<CheckoutPurchase> purchases,
            long itemTotal,
            long shippingTotal,
            long total,
            string? currency,
            string? createdAt
        ) {
            Id = id;
            BillingName = billingName;
            Email = email;
            Purchases = purchases;
            ItemTotal = itemTotal;
            ShippingTotal = shippingTotal;
            Total = total;
            Currency = currency;
            CreatedAt = createdAt;
        }
    }
}
