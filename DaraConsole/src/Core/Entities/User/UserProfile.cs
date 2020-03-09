namespace DaraBot.Core.Entities.User
{
    public struct UserProfile
    {
        public string Name { get; internal set; }
        public string Email { get; internal set; }
        public string PhoneNumber { get; internal set; }
        public Address address { get; internal set; }
        public CardDetails CardDetails { get; internal set; }

        public UserProfile(string name,
                           string email,
                           string phoneNumber,
                           Address address,
                           CardDetails cardDetails)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            this.address = address;
            CardDetails = cardDetails;
        }
    }
}