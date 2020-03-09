namespace DaraBot.Core.Entities
{
    public struct Profile
    {
        public string Name { get; }
        public User User { get; }
        public Address address { get; }
        public CardDetails CardDetails { get; }

        public Profile(string name,
                       User user,
                       Address address,
                       CardDetails cardDetails)
        {
            Name = name;
            User = user;
            this.address = address;
            CardDetails = cardDetails;
        }
    }
}