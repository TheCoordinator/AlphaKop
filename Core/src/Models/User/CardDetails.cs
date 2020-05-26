using AlphaKop.Core.CreditCard;

namespace AlphaKop.Core.Models.User {
    public struct CardDetails {
        public CreditCardData CardData { get; }
        public string ExpiryMonth { get; }
        public string ExpiryYear { get; }
        public string Verification { get; }

        public CardDetails(
            CreditCardData cardData,
            string expiryMonth,
            string expiryYear,
            string verification
        ) {
            CardData = cardData;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Verification = verification;
        }
    }
}
