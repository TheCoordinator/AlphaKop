namespace AlphaKop.Core.Models.User {
    public struct CardDetails {
        public string CardNumber { get; }
        public string ExpiryMonth { get; }
        public string ExpiryYear { get; }
        public string Verification { get; }

        public CardDetails(
            string cardNumber,
            string expiryMonth,
            string expiryYear,
            string verification
        ) {
            CardNumber = cardNumber;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Verification = verification;
        }
    }
}
