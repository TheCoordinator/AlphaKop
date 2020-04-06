using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Core.Models.User;
using AlphaKop.Supreme.Flows;
using CsvHelper;
using CsvHelper.Configuration;

namespace AlphaKop.ConsoleApp.Csv {
    sealed class SupremeParser {
        private readonly string SupremeTaskFilePath;

        public SupremeParser(string supremeTaskFilePath) {
            SupremeTaskFilePath = supremeTaskFilePath;
        }

        public IEnumerable<SupremeJob> Parse() {
            using (var reader = new StreamReader(SupremeTaskFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                Configure(csv.Configuration);

                return csv.GetRecords<SupremeJobCsv>()
                    .Select(ToSupremeJob);
            }
        }

        private void Configure(IReaderConfiguration config) {
            config.Delimiter = ",";
            config.AllowComments = false;
            config.HasHeaderRecord = true;
        }

        private SupremeJob ToSupremeJob(SupremeJobCsv csv) {
            throw new NotImplementedException();
        }
    }
}