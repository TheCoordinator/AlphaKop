using System;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchPookyStep : ITaskStep<PookyStepParameter, SupremeJob> { }

    sealed class FetchPookyStep : BaseStep<PookyStepParameter>, IFetchPookyStep {
        private readonly IPookyRepository pookyRepository;
        private readonly ILogger<FetchPookyStep> logger;

        public FetchPookyStep(
            IPookyRepository pookyRepository,
            IServiceProvider provider,
            ILogger<FetchPookyStep> logger
        ) : base(provider) {
            this.pookyRepository = pookyRepository;
            this.logger = logger;
        }

        protected override async Task Execute(PookyStepParameter parameter, SupremeJob job) {
            try {
                var pookyRegion = PookyRegionUtil.From(job.Region);
                var pooky = await pookyRepository.FetchPooky(pookyRegion);

                logger.LogInformation(
                    JobEventId,
                    $"Fetched Pooky {parameter.Item.Id}"
                );

                var addBasketParam = new AddBasketStepParameter(
                    item: parameter.Item,
                    style: parameter.Style,
                    size: parameter.Size,
                    pooky: pooky
                );
                
                await provider.CreateAddBasketStep(job)
                    .Execute(addBasketParam);
                    
            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "Failed to Fetch Pooky");

                await provider.CreateFetchPookyStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }
    }
}
