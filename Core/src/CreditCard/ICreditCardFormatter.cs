namespace AlphaKop.Core.CreditCard {
    public interface ICreditCardFormatter {
        string FormatCardNumber(CreditCardData cardData);
    }
}