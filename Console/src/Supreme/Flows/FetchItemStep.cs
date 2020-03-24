using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Core;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchItemStep : ITaskStep<Unit, SupremeJob> { }

    sealed class FetchItemStep : IFetchItemStep {
        private readonly ISupremeRepository supremeRepository;
        private readonly IServiceProvider provider;
        private readonly ILogger<FetchItemStep> logger;

        public SupremeJob? Job { get; set; }
        
        public FetchItemStep(
            ISupremeRepository supremeRepository,
            IServiceProvider provider,
            ILogger<FetchItemStep> logger
        ) {
            this.provider = provider;
            this.supremeRepository = supremeRepository;
            this.logger = logger;
        }

        public async Task Execute(Unit parameter) {
            if (Job == null) {
                throw new ArgumentNullException("Job");
            }

            try {
                var stock = await supremeRepository.FetchStock();
                var item = FindItem(stock: stock);

                await provider.CreateFetchItemDetailsStep(Job.Value)
                    .Execute(item);

            } catch (Exception ex) {
                logger.LogError(ex, "Failed to retrieve Item");

                await provider.CreateFetchItemStep(Job.Value)
                    .Execute(parameter);
            }
        }

        private Item FindItem(Stock stock) {
            // TODO: Fix Keywords

            var keywords = Job?.Keywords ?? "";
            var items = stock.Items;
            var selected = items
                .Select(
                    x => x.Value
                        .Where(x => x.Name.ToLower().Contains(keywords))
                );

            var item = selected.First()?.First();
            if (item == null) {
                throw new NullReferenceException("Could not find item");
            }

            return item.Value;
        }
    }
}
