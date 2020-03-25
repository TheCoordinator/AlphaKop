using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Core;
using AlphaKop.Core.Flows;
using AlphaKop.Core.Services.TextMatching;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchItemStep : ITaskStep<Unit, SupremeJob> { }

    sealed class FetchItemStep : IFetchItemStep {
        private readonly ISupremeRepository supremeRepository;
        private readonly ITextMatching textMatching;
        private readonly IServiceProvider provider;
        private readonly ILogger<FetchItemStep> logger;

        public SupremeJob? Job { get; set; }

        public FetchItemStep(
            ISupremeRepository supremeRepository,
            ITextMatching textMatching,
            IServiceProvider provider,
            ILogger<FetchItemStep> logger
        ) {
            this.supremeRepository = supremeRepository;
            this.textMatching = textMatching;
            this.provider = provider;
            this.logger = logger;
        }

        public async Task Execute(Unit parameter) {
            if (Job == null) {
                throw new ArgumentNullException("Job");
            }

            await Task.Delay(Job.Value.StartDelay);

            try {
                var stock = await supremeRepository.FetchStock();
                var item = FindItem(stock: stock, job: Job.Value);

                await provider.CreateFetchItemDetailsStep(Job.Value)
                    .Execute(item);

            } catch (Exception ex) {
                logger.LogError(Job.Value.ToEventId(), ex, "Failed to retrieve Item");

                await provider.CreateFetchItemStep(Job.Value)
                    .Execute(parameter);
            }
        }

        private Item FindItem(Stock stock, SupremeJob job) {
            var items = GetAllItems(stock: stock);
            var names = items.Select(item => item.Name);

            var results = textMatching.ExtractAll(
                query: job.Keywords,
                choices: names
            );

            if (results.Count() == 0) {
                throw new NullReferenceException("Could not find item");
            }

            logger.LogDebug(
                job.ToEventId(),
                $"Extracted {results.Count()} Items for keywords {job.Keywords}\n" +
                string.Join("\n", results.Select(i => i.ToString()))
            );

            var foundItems = ConvertResults(
                allItems: items,
                results: results
            );

            logger.LogDebug(
                job.ToEventId(),
                $"Extracted {foundItems.Count()} Items for keywords {job.Keywords}\n" +
                string.Join("\n\n", foundItems.Select(i => i.ToString()))
            );

            var item = SelectItem(
                job: job,
                items: foundItems,
                results: results
            );

            logger.LogDebug(job.ToEventId(), $"Selected Item {item.Id}");

            return item;
        }

        private IEnumerable<Item> GetAllItems(Stock stock) {
            return stock.Items
                .SelectMany(pair => pair.Value
                            .Select(item => item))
                .Distinct();

        }

        private Item SelectItem(
            SupremeJob job,
            IEnumerable<Item> items,
            IEnumerable<ExtractedResult<string>> results
        ) {
            if (job.CategoryName == null) {
                return items.First();
            }

            var categoryName = job.CategoryName.ToLower();
            Item? item = items.First(item => item.CategoryName?.ToLower() == categoryName);

            if (item == null) {
                throw new NullReferenceException("Could not find item");
            }

            return item.Value;
        }

        private IEnumerable<Item> ConvertResults(
            IEnumerable<Item> allItems,
            IEnumerable<ExtractedResult<string>> results
        ) {
            return results
                .Select(result => allItems
                    .First(item => item.Name == result.Value)
                );
        }
    }
}
