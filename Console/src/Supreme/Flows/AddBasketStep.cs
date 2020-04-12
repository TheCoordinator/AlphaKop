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
        private const int maxRetries = 20;
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
                        .Execute(parameter.SelectedItem.Item);

                    return;
                }

                var request = new AddBasketRequest(
                    itemId: parameter.SelectedItem.Item.Id,
                    sizeId: parameter.SelectedItem.Size.Id,
                    styleId: parameter.SelectedItem.Style.Id,
                    quantity: 1,
                    pooky: parameter.Pooky
                );

                var response = await supremeRepository.AddBasket(request);

                LogResponse(response, parameter);

                if (response.ItemSizesStock.Any(r => r.InStock == true) && response.Ticket != null) {
                    var pookyTicketParam = new PookyTicketStepParameter(
                        selectedItem: parameter.SelectedItem,
                        basketResponse: response,
                        basketTicket: response.Ticket,
                        pooky: parameter.Pooky
                    );

                    await provider.CreateFetchPookyTicketStep(job)
                        .Execute(pookyTicketParam);
                } else {
                    await provider.CreateAddBasketStep(job, Retries + 1)
                        .Execute(parameter);
                }
            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "--[AddBasket] Error");

                await provider.CreateAddBasketStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }

        private void LogResponse(AddBasketResponse response, AddBasketStepParameter parameter) {
            var selectedItem = parameter.SelectedItem;
            logger.LogInformation(
                JobEventId,
                $@"--[AddBasket] Status [In Stock] {parameter.SelectedItem.ToString()}"
            );
        }
    }
}
