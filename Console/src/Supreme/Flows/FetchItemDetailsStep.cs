using System;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchItemDetailsStep : ITaskStep<Item, SupremeJob> { }

    sealed class FetchItemDetailsStep : IFetchItemDetailsStep {        
        private readonly ISupremeRepository supremeRepository;
        private readonly ILogger<FetchItemDetailsStep> logger;
        private readonly IServiceProvider provider;

        public SupremeJob? Job { get; set; }

        public FetchItemDetailsStep(
            ISupremeRepository supremeRepository,
            ILogger<FetchItemDetailsStep> logger,
            IServiceProvider provider
        ) {
            this.supremeRepository = supremeRepository;
            this.logger = logger;
            this.provider = provider;
        }

        public async Task Execute(Item parameter) {
            if (Job == null) {
                throw new ArgumentNullException("Job");
            }

            try {
                var itemDetails = await supremeRepository.FetchItemDetails(item: parameter);

                logger.LogDebug($"Fetched Item Details");
            } catch (Exception ex) {
                logger.LogError(ex, "Failed to retrieve ItemDetails");

                await provider.CreateFetchItemDetailsStep(Job.Value)
                    .Execute(parameter);
            }
        }
    }
}
