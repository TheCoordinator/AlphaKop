namespace DaraBot.Core.Entities.User
{
    public struct Address
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string LineOne { get; }
        public string? LineTwo { get; }
        public string? LineThree { get; }
        public string City { get; }
        public string? State { get; }
        public string CountryCode { get; }
        public string PostCode { get; }

        public Address(string firstName,
                       string lastName,
                       string lineOne,
                       string lineTwo,
                       string lineThree,
                       string city,
                       string state,
                       string countryCode,
                       string postCode)
        {
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