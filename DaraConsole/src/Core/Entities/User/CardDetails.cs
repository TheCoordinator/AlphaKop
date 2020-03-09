namespace DaraBot.Core.Entities.User
{
    public struct CardDetails
    {
        public string CardNumber { get; internal set; }
        public string ExpiryMonth { get; internal set; }
        public string ExpiryYear { get; internal set; }
        public string CardVerification { get; internal set; }
    }
}