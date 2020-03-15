namespace AlphaKop.Core.Models.User {
    public struct Address {
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string LineOne { get; internal set; }
        public string? LineTwo { get; internal set; }
        public string? LineThree { get; internal set; }
        public string City { get; internal set; }
        public string? State { get; internal set; }
        public string CountryCode { get; internal set; }
        public string PostCode { get; internal set; }

        public Address(
            string firstName,
            string lastName,
            string lineOne,
            string lineTwo,
            string lineThree,
            string city,
            string state,
            string countryCode,
            string postCode
        ) {
            FirstName = firstName;
            LastName = lastName;
            LineOne = lineOne;
            LineTwo = lineTwo;
            LineThree = lineThree;
            City = city;
            State = state;
            CountryCode = countryCode;
            PostCode = postCode;
        }
    }
}
