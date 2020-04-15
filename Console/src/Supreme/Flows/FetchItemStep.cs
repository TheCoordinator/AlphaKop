using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Core.Services.TextMatching;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchItemStep : ITaskStep<InitialStepInput> { }

    public sealed class FetchItemStep : IFetchItemStep {
        private readonly ISupremeRepository supremeRepository;
        private readonly ITextMatching textMatching;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public int Retries { get; set; }

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

        public async Task Execute(InitialStepInput input) {
            try {
                await Task.Delay(input.Job.StartDelay);

                var stock = await supremeRepository.FetchStock();
                var item = FindItem(stock: stock, job: input.Job);

                logger.LogInformation(input.Job.ToEventId(), $"--FetchItemStep Item Fetched. [{item.Id}, {item.Name}] Keywords [{input.Job.Keywords}]");

                await PerformItemDetailsStep(item, input);
            } catch (ItemNotFoundException ex) {
                logger.LogInformation(input.Job.ToEventId(), $"--FetchItemStep Item Not Found. Keywords [{ex.Keywords}]");

                await RetryStep(input);
            } catch (Exception ex) {
                logger.LogError(input.Job.ToEventId(), ex, "--FetchItemStep Unhandled Exception");

                await RetryStep(input);
            }
        }

        private async Task PerformItemDetailsStep(Item item, InitialStepInput input) {
            await provider.CreateFetchItemDetailsStep(input.Job)
                .Execute(item);
        }

        private async Task RetryStep(InitialStepInput input) {
            await provider.CreateStep<InitialStepInput, IFetchItemStep>(Retries + 1)
                .Execute(input);
        }

        private Item FindItem(Stock stock, SupremeJob job) {
            var items = GetAllItems(stock: stock);
            var names = items.Select(item => item.Name);

            var results = textMatching.ExtractAll(query: job.Keywords, choices: names);

            if (results.Count() == 0) {
                throw new ItemNotFoundException(null, keywords: job.Keywords);
            }

            var foundItems = ConvertResults(allItems: items, results: results);

            return SelectItem(
                job: job,
                items: foundItems,
                results: results
            );
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
                return items.FirstOrDefault();
            }

            var categoryName = job.CategoryName.ToLower();
            Item? item = items.FirstOrDefault(item => item.CategoryName?.ToLower() == categoryName);

            if (item == null) {
                throw new ItemNotFoundException(null, keywords: job.Keywords);
            }

            return item.Value;
        }

        private IEnumerable<Item> ConvertResults(
            IEnumerable<Item> allItems,
            IEnumerable<ExtractedResult<string>> results
        ) {
            return results
                .Select(result => allItems
                    .FirstOrDefault(item => item.Name == result.Value)
                );
        }
    }
}
