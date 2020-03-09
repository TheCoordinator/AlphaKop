namespace DaraBot.Core.Entities
{
    public struct User
    {
        public string Email { get; }
        public string PhoneNumber { get; }

        public User(string email, string phoneNumber)
        {
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}