using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Repositories;
using AlphaKop.Supreme.Network;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IAddBasketStep : ITaskStep<AddBasketStepParameter, SupremeJob> { }

    sealed class AddBasketStep : BaseStep<AddBasketStepParameter>, IAddBasketStep {
        private const int maxRetries = 10;
        private readonly ISupremeRepository supremeRepository;
        private readonly ILogger<AddBasketStep> logger;

        public AddBasketStep(
            ISupremeRepository supremeRepository,
            IServiceProvider provider,
            ILogger<AddBasketStep> logger
        ) : base(provider) {
            this.supremeRepository = supremeRepository;
            this.logger = logger;
        }

        protected override async Task Execute(AddBasketStepParameter parameter, SupremeJob job) {
            try {
                if (Retries >= maxRetries) {
                    await provider.CreateFetchItemDetailsStep(job)
                        .Execute(parameter.Item);

                    return;
                }

                var request = new AddBasketRequest(
                    itemId: parameter.Item.Id,
                    sizeId: parameter.Size.Id,
                    styleId: parameter.Style.Id,
                    quantity: 1,
                    pooky: parameter.Pooky
                );

                var response = await supremeRepository.AddBasket(request);

                logger.LogInformation(
                    JobEventId,
                    $"Add Basket Response Item: {request.ItemId} Style: {request.StyleId}, Size: {request.SizeId}\n" +
                    string.Join("\n", response.Select(r => r.ToString()))
                );

                if (response.Any(r => r.InStock == true)) {
                    // TODO: Add Cookies

                    var captchaParam = new CaptchaStepParameter(
                        item: parameter.Item,
                        style: parameter.Style,
                        size: parameter.Size,
                        pooky: parameter.Pooky
                    );

                    await provider.CreateCaptchaStep(job)
                        .Execute(captchaParam);
                } else {
                    await provider.CreateAddBasketStep(job, Retries + 1)
                        .Execute(parameter);
                }
            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "Failed to retrieve AddBasket");

                await provider.CreateAddBasketStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }
    }
}
