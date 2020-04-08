namespace AlphaKop.Core.CreditCard {
    public interface ICreditCardValidator {
        CreditCardData? GetCardData(string cardNumber);
    }
}