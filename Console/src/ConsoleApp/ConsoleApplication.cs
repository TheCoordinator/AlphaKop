using AlphaKop.ConsoleApp.Csv;
using AlphaKop.Core.CreditCard;
using AlphaKop.Supreme.Flows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaKop.ConsoleApp {
    public sealed class ConsoleApplication {
        private readonly IServiceProvider provider;
        private readonly ILogger logger;
        public string CsvTaskPath { get; set; } = string.Empty;

        public ConsoleApplication(
            IServiceProvider provider,
            ILogger<ConsoleApplication> logger
        ) {
            this.provider = provider;
            this.logger = logger;
        }

        public async void Run() {
            logger.LogDebug("Starting Application");

            if (CsvTaskPath.Length == 0) {
                throw new ArgumentException("Task path not provided");
            }

            var supremeParser = new SupremeCsvParser(CsvTaskPath, creditCardValidator: provider.GetRequiredService<ICreditCardValidator>());

            var parsedJobs = supremeParser
                .Parse();

            logger.LogInformation($"Loaded {parsedJobs.Count()} Jobs");

            var tasks = parsedJobs
                .Select(job => {
                    var task = provider.GetRequiredService<ISupremeStartStep>();
                    return task.Execute(input: new InitialStepInput(job));
                });

            await Task.WhenAll(tasks);
        }
    }
}