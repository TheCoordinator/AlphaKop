namespace DaraBot.Core.Entities.User
{
    public struct CardDetails
    {
        public string CardNumber { get; }
        public string ExpiryMonth { get; }
        public string ExpiryYear { get; }
        public string CardVerification { get; }
    }
}