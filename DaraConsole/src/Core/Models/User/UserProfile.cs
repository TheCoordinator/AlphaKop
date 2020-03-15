namespace DaraBot.Core.Models.User {
    public struct UserProfile {
        public string Name { get; internal set; }
        public string Email { get; internal set; }
        public string PhoneNumber { get; internal set; }
        public Address Address { get; internal set; }
        public CardDetails CardDetails { get; internal set; }

        public UserProfile (
            string name,
            string email,
            string phoneNumber,
            Address address,
            CardDetails cardDetails
        ) {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            CardDetails = cardDetails;
        }
    }
}
