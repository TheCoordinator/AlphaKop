using System.Collections.Generic;
using AlphaKop.Core.Flows;
using AlphaKop.Core.Models.User;

namespace AlphaKop.Supreme.Flows {
    public struct SupremeJob: IJob {
        public UserProfile Profile { get; internal set; }
        public string JobId { get; internal set; }
        public int JobEventId { get; internal set; }
        public string? CategoryName { get; internal set; }
        public string Keywords { get; internal set; }
        public string? Style { get; internal set; }
        public string? Size { get; internal set; }
        public int CheckoutDelay { get; internal set; }
        public int StartDelay { get; internal set; }

        public SupremeJob(
            UserProfile profile,
            string jobId,
            int jobEventId,
            string? categoryName,
            string keywords,
            string? style,
            string? size,
            int checkoutDelay = 2000,
            int startDelay = 1000
        ) {
            Profile = profile;
            JobId = jobId;
            JobEventId = jobEventId;
            CategoryName = categoryName;
            Keywords = keywords;
            Style = style;
            Size = size;
            CheckoutDelay = checkoutDelay;
            StartDelay = startDelay;
        }
    }
}
