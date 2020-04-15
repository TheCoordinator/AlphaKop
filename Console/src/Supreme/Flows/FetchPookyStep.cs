using System;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchPookyStep : ITaskStep<PookyStepInput> { }

    sealed class FetchPookyStep : IFetchPookyStep {
        private const int maxRetries = 10;
        private readonly IPookyRepository pookyRepository;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public int Retries { get; set; }

        public FetchPookyStep(
            IPookyRepository pookyRepository,
            IServiceProvider provider,
            ILogger<FetchPookyStep> logger
        ) {
            this.pookyRepository = pookyRepository;
            this.provider = provider;
            this.logger = logger;
        }

        public async Task Execute(PookyStepInput input) {
            if (Retries >= maxRetries) {
                await RevertToItemDetailsStep(input);
                return;
            }

            try {
                await Task.Delay(input.Job.StartDelay);

                var pookyRegion = PookyRegionUtil.From(input.Job.Region);
                var pooky = await pookyRepository.FetchPooky(pookyRegion);

                await PerformPostPooky(input, pooky);
            } catch (Exception ex) {
                logger.LogError(input.Job.ToEventId(), ex, "--[FetchPookyStep] Unhandled Exception");

                await RetryStep(input);
            }
        }

        private async Task PerformPostPooky(PookyStepInput input, Pooky pooky) {
            LogPooky(input, pooky);
            await PerformAddBasketStep(input, pooky);
        }

        private async Task PerformAddBasketStep(PookyStepInput input, Pooky pooky) {
            var addBasketParam = new AddBasketStepParameter(
                selectedItem: input.SelectedItem,
                pooky: pooky
            );

            await provider.CreateAddBasketStep(input.Job)
                .Execute(addBasketParam);
        }

        private async Task RevertToItemDetailsStep(PookyStepInput input) {
            var itemDetailsInput = new ItemDetailsStepInput(
                item: input.SelectedItem.Item,
                job: input.Job
            );

            await provider.CreateStep<ItemDetailsStepInput, IFetchItemDetailsStep>()
                .Execute(itemDetailsInput);
        }

        private async Task RetryStep(PookyStepInput input) {
            await provider.CreateStep<PookyStepInput, IFetchPookyStep>(Retries + 1)
                .Execute(input);
        }

        private void LogPooky(PookyStepInput input, Pooky pooky) {
            logger.LogInformation(
                input.Job.ToEventId(),
                $@"--[FetchPookyStep] Status [Fetched] {input.SelectedItem.ToString()}"
            );
        }
    }
}
