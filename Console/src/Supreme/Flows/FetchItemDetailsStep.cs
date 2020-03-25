using System;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Core.Services.TextMatching;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchItemDetailsStep : ITaskStep<Item, SupremeJob> { }

    sealed class FetchItemDetailsStep : BaseStep<Item>, IFetchItemDetailsStep {        
        private readonly ISupremeRepository supremeRepository;
        private readonly ITextMatching textMatching;
        private readonly ILogger<FetchItemDetailsStep> logger;

        public FetchItemDetailsStep(
            ISupremeRepository supremeRepository,
            ITextMatching textMatching,
            IServiceProvider provider,
            ILogger<FetchItemDetailsStep> logger
        ) : base(provider) {
            this.supremeRepository = supremeRepository;
            this.textMatching = textMatching;
            this.logger = logger;
        }

        protected override async Task Execute(Item parameter, SupremeJob job) {            
            try {
                var itemDetails = await supremeRepository.FetchItemDetails(item: parameter);

                logger.LogInformation(
                    JobEventId, 
                    $"Fetched Item Details {parameter.Id}\n" +
                    itemDetails.ToString()
                );
            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "Failed to retrieve ItemDetails");

                await provider.CreateFetchItemDetailsStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }
    }
}
