using AlphaKop.Core.Models.User;

namespace AlphaKop.Supreme.Flows {
    public struct SupremeJob {
        public UserProfile Profile { get; internal set; }
        public string? CategoryName { get; internal set; }
        public string? Keywords { get; internal set; }
        public string? Style { get; internal set; }
        public string? Size { get; internal set; }
        public int CheckoutDelay { get; internal set; }
        public int RetryDelay { get; internal set; }

        public SupremeJob(
            UserProfile profile,
            string? categoryName,
            string? keywords,
            string? style,
            string? size,
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
