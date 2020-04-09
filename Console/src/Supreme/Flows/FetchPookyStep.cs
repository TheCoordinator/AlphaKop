using System;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchPookyStep : ITaskStep<SelectedItemParameter, SupremeJob> { }

    sealed class FetchPookyStep : BaseStep<SelectedItemParameter>, IFetchPookyStep {
        private const int maxRetries = 10;
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

        protected override async Task Execute(SelectedItemParameter parameter, SupremeJob job) {
            if (Retries >= maxRetries) {
                await provider.CreateFetchItemDetailsStep(job)
                    .Execute(parameter.Item);

                return;
            }

            try {
                var pookyRegion = PookyRegionUtil.From(job.Region);
                var pooky = await pookyRepository.FetchPooky(pookyRegion);

                LogResponse(pooky, parameter);

                var addBasketParam = new AddBasketStepParameter(
                    selectedItem: parameter,
                    pooky: pooky
                );

                await provider.CreateAddBasketStep(job)
                    .Execute(addBasketParam);

            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "--[FetchPooky] Error");

                await provider.CreateFetchPookyStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }

        private void LogResponse(Pooky response, SelectedItemParameter parameter) {
            logger.LogInformation(
                JobEventId,
                $@"--[FetchPooky] Status [Fetched] {parameter.ToString()}"
            );
        }
    }
}
