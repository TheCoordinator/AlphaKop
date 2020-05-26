using Microsoft.Extensions.Logging;

namespace AlphaKop.Core.Flows {
    public static class IJobExtensions {
        public static EventId ToEventId(this IJob job) {
            return new EventId(id: job.JobEventId, name: job.JobId);
        }
    }
}