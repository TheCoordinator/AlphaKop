using System;
using AlphaKop.Core.Flows;
using AlphaKop.Core.Models.User;

namespace AlphaKop.Supreme.Flows {
    public struct SupremeJob : IJob {
        public string JobId { get; }
        public int JobEventId { get; }

        public UserProfile Profile { get; }
        public string Region { get; }

        public string? CategoryName { get; }
        public string Keywords { get; }
        public string? Style { get; }
        public string? Size { get; }
        public int Quantity { get; }
        public bool FastMode { get; }
        public bool IsCard3DSecureEnabled { get; }
        public int StartDelay { get; }

        public SupremeJob(
            UserProfile profile,
            string region,
            string jobId,
            int jobEventId,
            string? categoryName,
            string keywords,
            string? style,
            string? size,
            int quantity,
            bool fastMode = false,
            bool isCard3DSecureEnabled = false,
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
            Quantity = Math.Max(1, quantity);
            FastMode = fastMode;
            IsCard3DSecureEnabled = isCard3DSecureEnabled;
            StartDelay = startDelay;
        }
    }
}
