using DaraBot.Core.Entities.User;

namespace DaraBot.Supreme.Entities
{
    public struct SupremeTask
    {
        public UserProfile Profile { get; internal set; }
        public string? CategoryName { get; internal set; }
        public string? Keywords { get; internal set; }
        public string? Style { get; internal set; }
        public string? Size { get; internal set; }
        public int CheckoutDelay { get; internal set; }
        public int RetryDelay { get; internal set; }

        public SupremeTask(UserProfile profile,
                           string categoryName,
                           string keywords,
                           string style,
                           string size,
                           int checkoutDelay = 1000,
                           int retryDelay = 1000)
        {
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