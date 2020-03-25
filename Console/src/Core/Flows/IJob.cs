namespace AlphaKop.Core.Flows {
    public interface IJob {
        string JobId { get; }
        int JobEventId { get; }
    }
}
