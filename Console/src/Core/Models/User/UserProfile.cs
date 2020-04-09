namespace AlphaKop.Core.Models.User {
    public struct UserProfile {
        public string Name { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public Address Address { get; }
        public CardDetails CardDetails { get; }

        public UserProfile(
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
