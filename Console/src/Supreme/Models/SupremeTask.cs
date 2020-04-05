using AlphaKop.Core.Models.User;

namespace AlphaKop.Supreme.Models {
    public struct SupremeTask {
        public UserProfile Profile { get; }
        public string? CategoryName { get; }
        public string? Keywords { get; }
        public string? Style { get; }
        public string? Size { get; }
        public int CheckoutDelay { get; }
        public int RetryDelay { get; }

        public SupremeTask(
            UserProfile profile,
            string categoryName,
            string keywords,
            string style,
            string size,
            int checkoutDelay = 1000,
            int retryDelay = 1000
        ) {
            Profile = profile;
            CategoryName = categoryName;
            Keywords = keywords;
            Style = style;
            Size = size;
            CheckoutDelay = checkoutDelay;
            RetryDelay = retryDelay;
        }
    }
}
