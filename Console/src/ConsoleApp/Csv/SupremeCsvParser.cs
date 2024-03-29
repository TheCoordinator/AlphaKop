using AlphaKop.Core.CreditCard;
using AlphaKop.Core.Models.User;
using AlphaKop.Core.System.Extensions;
using AlphaKop.Supreme.Flows;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AlphaKop.ConsoleApp.Csv {
    sealed class SupremeCsvParser {
        private readonly string SupremeTaskFilePath;
        private readonly ICreditCardValidator creditCardValidator;

        public SupremeCsvParser(string supremeTaskFilePath, ICreditCardValidator creditCardValidator) {
            SupremeTaskFilePath = supremeTaskFilePath;
            this.creditCardValidator = creditCardValidator;
        }

        public IEnumerable<SupremeJob> Parse() {
            using (var reader = new StreamReader(SupremeTaskFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                Configure(csv.Configuration);

                return csv.GetRecords<SupremeJobCsv>()
                    .ToList()
                    .Select((csvValue, index) => {
                        return ToSupremeJob(csvValue, index);
                    })
                    .Where(job => job != null)
                    .Select(job => job.GetValueOrDefault());
            }
        }

        private void Configure(IReaderConfiguration config) {
            config.Delimiter = ",";
            config.AllowComments = false;
            config.HasHeaderRecord = true;
        }

        private SupremeJob? ToSupremeJob(SupremeJobCsv csv, int index) {
            var profile = ToUserProfile(csv);

            if (profile == null) {
                return null;
            }

            return new SupremeJob(
                profile: profile.Value,
                region: csv.JobRegion,
                jobId: index.ToString(),
                jobEventId: index,
                categoryName: csv.JobCategoryName?.NullIfEmptyTrimmed(),
                keywords: csv.JobKeywords,
                style: csv.JobStyle?.NullIfEmptyTrimmed(),
                size: csv.JobSize?.NullIfEmptyTrimmed(),
                quantity: csv.JobQuantity,
                fastMode: csv.FastMode,
                isCard3DSecureEnabled: csv.IsCard3DSecureEnabled,
                startDelay: csv.JobStartDelay
            );
        }

        private UserProfile? ToUserProfile(SupremeJobCsv csv) {
            var cardDetails = ToCardDetails(csv);

            if (cardDetails == null) {
                return null;
            }

            return new UserProfile(
                name: csv.ProfileName.Trim(),
                email: csv.ProfileEmail.Trim().ToLower(),
                phoneNumber: csv.ProfilePhoneNumber.Trim().ToLower(),
                address: ToAddress(csv),
                cardDetails: cardDetails.Value
            );
        }

        private CardDetails? ToCardDetails(SupremeJobCsv csv) {
            var cardData = creditCardValidator.GetCardData(csv.CardNumber);

            if (cardData == null) {
                return null;
            }

            return new CardDetails(
                cardData: cardData.Value,
                expiryMonth: csv.CardExpiryMonth.Trim(),
                expiryYear: csv.CardExpiryYear.Trim(),
                verification: csv.CardVerification.Trim()
            );
        }

        private Address ToAddress(SupremeJobCsv csv) {
            return new Address(
                firstName: csv.AddressFirstName.Trim(),
                lastName: csv.AddressLastName.Trim(),
                lineOne: csv.AddressLineOne.Trim(),
                lineTwo: csv.AddressLineTwo?.NullIfEmptyTrimmed(),
                lineThree: csv.AddressLineThree?.NullIfEmptyTrimmed(),
                city: csv.AddressCity.Trim(),
                state: csv.AddressState?.NullIfEmptyTrimmed(),
                countryCode: csv.AddressCountryCode.Trim().ToUpper(),
                postCode: csv.AddressPostCode.Trim()
            );
        }
    }
}