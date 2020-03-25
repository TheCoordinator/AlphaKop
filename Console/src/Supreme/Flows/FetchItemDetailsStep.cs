using System;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchItemDetailsStep : ITaskStep<Item, SupremeJob> { }

    sealed class FetchItemDetailsStep : BaseStep<Item>, IFetchItemDetailsStep {        
        private readonly ISupremeRepository supremeRepository;
        private readonly ILogger<FetchItemDetailsStep> logger;

        public FetchItemDetailsStep(
            ISupremeRepository supremeRepository,
            IServiceProvider provider,
            ILogger<FetchItemDetailsStep> logger
        ) : base(provider) {
            this.supremeRepository = supremeRepository;
            this.logger = logger;
        }

        protected override async Task Execute(Item parameter, SupremeJob job) {            
            try {
                var itemDetails = await supremeRepository.FetchItemDetails(item: parameter);

                logger.LogDebug($"Fetched Item Details");
            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "Failed to retrieve ItemDetails");

                await provider.CreateFetchItemDetailsStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }
    }
}
