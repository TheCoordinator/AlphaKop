using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AlphaKop.Core.Models.User;
using AlphaKop.Core.System.Extensions;
using AlphaKop.Supreme.Flows;
using CsvHelper;
using CsvHelper.Configuration;

namespace AlphaKop.ConsoleApp.Csv {
    sealed class SupremeCsvParser {
        private readonly string SupremeTaskFilePath;

        public SupremeCsvParser(string supremeTaskFilePath) {
            SupremeTaskFilePath = supremeTaskFilePath;
        }

        public IEnumerable<SupremeJob> Parse() {
            using (var reader = new StreamReader(SupremeTaskFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                Configure(csv.Configuration);

                return csv.GetRecords<SupremeJobCsv>()
                    .ToList()
                    .Select((csvValue, index) => {
                        return ToSupremeJob(csvValue, index);
                    });
            }
        }

        private void Configure(IReaderConfiguration config) {
            config.Delimiter = ",";
            config.AllowComments = false;
            config.HasHeaderRecord = true;
        }

        private SupremeJob ToSupremeJob(SupremeJobCsv csv, int index) {
            return new SupremeJob(
                profile: ToUserProfile(csv),
                region: csv.JobRegion,
                jobId: index.ToString(),
                jobEventId: index,
                categoryName: csv.JobCategoryName?.NullIfEmptyTrimmed(),
                keywords: csv.JobKeywords,
                style: csv.JobStyle?.NullIfEmptyTrimmed(),
                size: csv.JobSize?.NullIfEmptyTrimmed(),
                quantity: csv.JobQuantity,
                startDelay: csv.JobStartDelay
            );
        }

        private UserProfile ToUserProfile(SupremeJobCsv csv) {
            return new UserProfile(
                name: csv.ProfileName,
                email: csv.ProfileEmail,
                phoneNumber: csv.ProfilePhoneNumber,
                address: ToAddress(csv),
                cardDetails: ToCardDetails(csv)
            );
        }

        private CardDetails ToCardDetails(SupremeJobCsv csv) {
            return new CardDetails(
                cardNumber: csv.CardNumber,
                expiryMonth: csv.CardExpiryMonth,
                expiryYear: csv.CardExpiryYear,
                verification: csv.CardVerification
            );
        }

        private Address ToAddress(SupremeJobCsv csv) {
            return new Address(
                firstName: csv.AddressFirstName,
                lastName: csv.AddressLastName,
                lineOne: csv.AddressLineOne,
                lineTwo: csv.AddressLineTwo?.NullIfEmptyTrimmed(),
                lineThree: csv.AddressLineThree?.NullIfEmptyTrimmed(),
                city: csv.AddressCity,
                state: csv.AddressState?.NullIfEmptyTrimmed(),
                countryCode: csv.AddressCountryCode,
                postCode: csv.AddressPostCode
            );
        }
    }
}