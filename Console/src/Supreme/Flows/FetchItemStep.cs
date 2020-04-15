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

    sealed class FetchItemStep : BaseStep<Unit>, IFetchItemStep {
        private readonly ISupremeRepository supremeRepository;
        private readonly ITextMatching textMatching;
        private readonly ILogger logger;

        public FetchItemStep(
            ISupremeRepository supremeRepository,
            ITextMatching textMatching,
            IServiceProvider provider,
            ILogger<FetchItemStep> logger
        ) : base(provider) {
            this.supremeRepository = supremeRepository;
            this.textMatching = textMatching;
            this.logger = logger;
        }

        protected override async Task Execute(Unit parameter, SupremeJob job) {
            try {
                var stock = await supremeRepository.FetchStock();
                var item = FindItem(stock: stock, job: job);

                logger.LogInformation(JobEventId, $"--FetchItemStep Item Fetched. [{item.Id}, {item.Name}] Keywords [{job.Keywords}]");

                await provider.CreateFetchItemDetailsStep(job)
                    .Execute(item);

            } catch (ItemNotFoundException ex) {
                logger.LogInformation(JobEventId, $"--FetchItemStep Item Not Found. Keywords [{ex.Keywords}]");

                await provider.CreateFetchItemStep(job, retries: Retries + 1)
                    .Execute(parameter);

            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "--FetchItemStep Unhandled Exception");

                await provider.CreateFetchItemStep(job, retries: Retries + 1)
                    .Execute(parameter);
            }
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
