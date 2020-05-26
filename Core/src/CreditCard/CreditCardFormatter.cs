using System;

namespace AlphaKop.Core.CreditCard {
    public sealed class CreditCardFormatter : ICreditCardFormatter {
        public string FormatCardNumber(CreditCardData cardData) {
            switch (cardData.Issuer) {
                case CardIssuer.AmericanExpress:
                    return String.Format("{0:0000 000000 00000}", (Int64.Parse(cardData.CardNumber)));
                default:
                    return String.Format("{0:0000 0000 0000 0000}", (Int64.Parse(cardData.CardNumber)));
            }
        }
    }
}