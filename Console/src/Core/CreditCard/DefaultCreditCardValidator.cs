using System;
using CreditCardValidator;

namespace AlphaKop.Core.CreditCard {
    public sealed class DefaultCreditCardValidator : ICreditCardValidator {
        public CreditCardData? GetCardData(string cardNumber) {
            var detector = new CreditCardDetector(cardNumber);

            if (detector.IsValid() == false) {
                return null;
            }

            var issuer = ConvertIssuer(brand: detector.Brand);
            var issuerName = detector.BrandName;

            return new CreditCardData(
                cardNumber: detector.CardNumber,
                issuer: issuer,
                issuerName: issuerName
            );
        }

        private CardIssuer ConvertIssuer(CreditCardValidator.CardIssuer brand) {
            switch (brand) {
                case CreditCardValidator.CardIssuer.Visa:
                    return CardIssuer.VISA;
                case CreditCardValidator.CardIssuer.MasterCard:
                    return CardIssuer.MasterCard;
                case CreditCardValidator.CardIssuer.AmericanExpress:
                    return CardIssuer.AmericanExpress;
                case CreditCardValidator.CardIssuer.Discover:
                    return CardIssuer.Discover;
                default:
                    return CardIssuer.Unknown;
            }
        }
    }
}