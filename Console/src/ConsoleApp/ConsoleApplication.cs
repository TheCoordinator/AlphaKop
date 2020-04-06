using System;
using System.Linq;
using AlphaKop.ConsoleApp.Csv;
using AlphaKop.Core.Models.User;
using AlphaKop.Supreme.Flows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AlphaKop.ConsoleApp {
    public sealed class ConsoleApplication {
        private readonly IServiceProvider provider;
        private readonly ILogger<ConsoleApplication> logger;
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

            var tasks = new SupremeCsvParser(CsvTaskPath)
                .Parse()
                .Select(job => {
                    var task = provider.GetRequiredService<ISupremeStartStep>();
                    task.Job = job;
                    return task;
                });

            foreach (var task in tasks) {
                await task.Execute(task.Job.GetValueOrDefault());
            }
        }
    }
}