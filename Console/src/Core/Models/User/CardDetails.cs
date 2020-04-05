namespace AlphaKop.Core.Models.User {
    public struct CardDetails {
        public string CardNumber { get; }
        public string ExpiryMonth { get; }
        public string ExpiryYear { get; }
        public string CardVerification { get; }

        public CardDetails(
            string cardNumber,
            string expiryMonth,
            string expiryYear,
            string cardVerification
        ) {
            CardNumber = cardNumber;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            CardVerification = cardVerification;
        }
    }
}
