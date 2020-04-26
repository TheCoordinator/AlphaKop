using CsvHelper.Configuration.Attributes;

namespace AlphaKop.ConsoleApp.Csv {
    public sealed class SupremeJobCsv {
        [Name("profile.name")]
        public string ProfileName { get; set; } = string.Empty;

        [Name("profile.email")]
        public string ProfileEmail { get; set; } = string.Empty;

        [Name("profile.phone_number")]
        public string ProfilePhoneNumber { get; set; } = string.Empty;

        [Name("profile.address.first_name")]
        public string AddressFirstName { get; set; } = string.Empty;

        [Name("profile.address.last_name")]
        public string AddressLastName { get; set; } = string.Empty;

        [Name("profile.address.line_one")]
        public string AddressLineOne { get; set; } = string.Empty;

        [Name("profile.address.line_two")]
        public string? AddressLineTwo { get; set; } = null;

        [Name("profile.address.line_three")]
        public string? AddressLineThree { get; set; } = null;

        [Name("profile.address.city")]
        public string AddressCity { get; set; } = string.Empty;

        [Name("profile.address.state")]
        public string? AddressState { get; set; } = null;

        [Name("profile.address.country_code")]
        public string AddressCountryCode { get; set; } = string.Empty;

        [Name("profile.address.postcode")]
        public string AddressPostCode { get; set; } = string.Empty;

        [Name("profile.card.card_number")]
        public string CardNumber { get; set; } = string.Empty;

        [Name("profile.card.expiry_month")]
        public string CardExpiryMonth { get; set; } = string.Empty;

        [Name("profile.card.expiry_year")]
        public string CardExpiryYear { get; set; } = string.Empty;

        [Name("profile.card.verification")]
        public string CardVerification { get; set; } = string.Empty;

        [Name("job.region")]
        public string JobRegion { get; set; } = "EU";

        [Name("job.category_name")]
        public string? JobCategoryName { get; set; } = null;

        [Name("job.keywords")]
        public string JobKeywords { get; set; } = string.Empty;

        [Name("job.style")]
        public string? JobStyle { get; set; } = null;

        [Name("job.size")]
        public string? JobSize { get; set; } = null;

        [Name("job.quantity")]
        public int JobQuantity { get; set; } = 1;

        [Name("job.fast_mode")]
        public bool FastMode { get; set; } = false;

        [Name("job.card_3d_secure")]
        public bool IsCard3DSecureEnabled { get; set; } = false;

        [Name("job.start_delay")]
        public int JobStartDelay { get; set; } = 1000;
    }
}
