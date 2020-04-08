namespace AlphaKop.Core.CreditCard {
    public struct CreditCardData {
        public string CardNumber { get; }
        public CardIssuer Issuer { get; }
        public string IssuerName { get; }

        public CreditCardData(string cardNumber, CardIssuer issuer, string issuerName) {
            CardNumber = cardNumber;
            Issuer = issuer;
            IssuerName = issuerName;
        }
    }
}