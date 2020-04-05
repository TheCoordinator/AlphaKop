using AlphaKop.Core.Flows;
using AlphaKop.Core.Models.User;

namespace AlphaKop.Supreme.Flows {
    public struct SupremeJob : IJob {
        public string JobId { get; }
        public int JobEventId { get; }

        public UserProfile Profile { get; }
        public SupremeRegion Region { get; }

        public string? CategoryName { get; }
        public string Keywords { get; }
        public string? Style { get; }
        public string? Size { get; }
        public int CheckoutDelay { get; }
        public int StartDelay { get; }

        public SupremeJob(
            UserProfile profile,
            SupremeRegion region,
            string jobId,
            int jobEventId,
            string? categoryName,
            string keywords,
            string? style,
            string? size,
            int checkoutDelay = 2000,
            int startDelay = 1000
        ) {
            JobId = jobId;
            JobEventId = jobEventId;
            Profile = profile;
            Region = region;
            CategoryName = categoryName;
            Keywords = keywords;
            Style = style;
            Size = size;
            CheckoutDelay = checkoutDelay;
            StartDelay = startDelay;
        }
    }
}
