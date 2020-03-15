namespace AlphaKop.Core.Models.User {
    public struct CardDetails {
        public string CardNumber { get; internal set; }
        public string ExpiryMonth { get; internal set; }
        public string ExpiryYear { get; internal set; }
        public string CardVerification { get; internal set; }

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
