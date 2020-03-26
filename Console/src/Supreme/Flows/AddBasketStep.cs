using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Core.Services.TextMatching;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using AlphaKop.Supreme.Requests;
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
                    // Next Step
                } else {
                    await provider.CreateAddBasketStep(job, Retries + 1)
                        .Execute(parameter);
                }
            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "Failed to retrieve ItemDetails");

                await provider.CreateAddBasketStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }
    }
}
