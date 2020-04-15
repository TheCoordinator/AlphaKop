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
        private readonly ILogger logger;

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
                await RevertToItemDetailsStep(parameter, job);
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

        private async Task RevertToItemDetailsStep(SelectedItemParameter itemParameter, SupremeJob job) {
            var itemDetailsInput = new ItemDetailsStepInput(
                item: itemParameter.Item,
                job: job
            );

            await provider.CreateStep<ItemDetailsStepInput, IFetchItemDetailsStep>()
                .Execute(itemDetailsInput);
        }

        private void LogResponse(Pooky response, SelectedItemParameter parameter) {
            logger.LogInformation(
                JobEventId,
                $@"--[FetchPooky] Status [Fetched] {parameter.ToString()}"
            );
        }
    }
}
