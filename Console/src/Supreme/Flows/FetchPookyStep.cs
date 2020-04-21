using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchPookyStep : ITaskStep<PookyStepInput> { }

    public sealed class FetchPookyStep : IFetchPookyStep {
        private const int maxRetries = 10;
        private const int delayInMilliSeconds = 200;

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
                await Task.Delay(delayInMilliSeconds);

                var pooky = await PerformFetchPooky(input);
                await PerformPostPooky(input, pooky);
            } catch (Exception ex) {
                logger.LogError(input.Job.ToEventId(), ex, "--[FetchPookyStep] Unhandled Exception");

                await RetryStep(input);
            }
        }

        private async Task<Pooky> PerformFetchPooky(PookyStepInput input) {
            var region = PookyRegionUtil.From(input.Job.Region);

            // TODO: decided based on job
            return await PerformFetchPooky(input, region);
            // return await PerformFetchPooky(region);
        }

        private async Task<Pooky> PerformFetchPooky(PookyRegion region) {
            return await pookyRepository.FetchPooky(region);
        }

        private async Task<Pooky> PerformFetchPooky(PookyStepInput input, PookyRegion region) {
            var request = new PookyItemRequest(
                region: region,
                styleId: input.SelectedItem.Style.Id,
                sizeId: input.SelectedItem.Size.Id
            );

            return await pookyRepository.FetchPooky(request);
        }

        private async Task PerformPostPooky(PookyStepInput input, Pooky pooky) {
            LogPooky(input, pooky);

            // TODO: Decide based on Job
            await PerformCaptchaStep(input, pooky);
            // await PerformAddBasketStep(input, pooky);
        }

        private async Task PerformAddBasketStep(PookyStepInput input, Pooky pooky) {
            var addBasketInput = new AddBasketStepInput(
                selectedItem: input.SelectedItem,
                pooky: pooky,
                job: input.Job
            );

            await provider.CreateStep<AddBasketStepInput, IAddBasketStep>()
                .Execute(addBasketInput);
        }

        private async Task PerformCaptchaStep(PookyStepInput input, Pooky pooky) {
            var checkoutCookies = new CheckoutCookies(
                new List<IEnumerable<Cookie>>() {
                    pooky.Cookies.StaticCookies,
                    pooky.Cookies.CheckoutCookies
                }
            );

            var captchaInput = new CaptchaStepInput(
                selectedItem: input.SelectedItem,
                pooky: pooky,
                checkoutCookies: checkoutCookies,
                job: input.Job
            );

            await provider.CreateStep<CaptchaStepInput, ICaptchaStep>()
                .Execute(captchaInput);
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
