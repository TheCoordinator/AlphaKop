using DaraBot.Core.Entities.User;

namespace DaraBot.Supreme.Entities
{
    public struct SupremeTask
    {
        public UserProfile Profile { get; internal set; }
        public string? CategoryName { get; internal set; }
        public string? keywords { get; internal set; }
        public string? style { get; internal set; }
        public string? size { get; internal set; }
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
            this.keywords = keywords;
            this.style = style;
            this.size = size;
            CheckoutDelay = checkoutDelay;
            RetryDelay = retryDelay;
        }
    }
}