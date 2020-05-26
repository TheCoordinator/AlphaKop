using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Network;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaKop.Supreme.Flows {
    public interface IAddBasketStep : ITaskStep<AddBasketStepInput> { }

    public sealed class AddBasketStep : IAddBasketStep {
        private const int maxRetries = 30;
        private const int delayInMilliSeconds = 200;

        private readonly ISupremeCheckoutRepository supremeRepository;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public int Retries { get; set; }

        public AddBasketStep(
            ISupremeCheckoutRepository supremeRepository,
            IServiceProvider provider,
            ILogger<AddBasketStep> logger
        ) {
            this.supremeRepository = supremeRepository;
            this.provider = provider;
            this.logger = logger;
        }

        public async Task Execute(AddBasketStepInput input) {
            try {
                if (Retries >= maxRetries) {
                    await RevertToItemDetailsStep(input);
                    return;
                }

                await Task.Delay(delayInMilliSeconds);

                var request = CreateAddBasketRequest(input);
                var response = await supremeRepository.AddBasket(request);

                await PerformPostAddBasket(input, response);
            } catch (Exception ex) {
                logger.LogError(input.Job.ToEventId(), ex, "--[AddBasketStep] Unhandled Exception");

                await RetryStep(input);
            }
        }

        private AddBasketRequest CreateAddBasketRequest(AddBasketStepInput input) {
            return new AddBasketRequest(
                itemId: input.SelectedItem.Item.Id,
                sizeId: input.SelectedItem.Size.Id,
                styleId: input.SelectedItem.Style.Id,
                quantity: Math.Min(input.Job.Quantity, 1),
                pooky: input.Pooky
            );
        }

        private async Task PerformPostAddBasket(AddBasketStepInput input, AddBasketResponse response) {
            var isInStock = response.ItemSizesStock.Any(r => r.InStock == true);
            LogResponse(input, response: response, isInStock: isInStock);

            if (isInStock && response.Ticket != null) {
                await PerformPookyTicketStep(input, response, response.Ticket);
            } else {
                await RetryStep(input);
            }
        }

        private async Task PerformPookyTicketStep(AddBasketStepInput input, AddBasketResponse response, string ticket) {
            var pookyTicketInput = new PookyTicketStepInput(
                selectedItem: input.SelectedItem,
                basketResponse: response,
                basketTicket: ticket,
                pooky: input.Pooky,
                job: input.Job
            );

            await provider.CreateStep<PookyTicketStepInput, IFetchPookyTicketStep>()
                .Execute(pookyTicketInput);
        }

        private async Task RetryStep(AddBasketStepInput input) {
            await provider.CreateStep<AddBasketStepInput, IAddBasketStep>(Retries + 1)
                .Execute(input);
        }

        private async Task RevertToItemDetailsStep(AddBasketStepInput input) {
            var itemDetailsInput = new ItemDetailsStepInput(
                item: input.SelectedItem.Item,
                job: input.Job
            );

            await provider.CreateStep<ItemDetailsStepInput, IFetchItemDetailsStep>()
                .Execute(itemDetailsInput);
        }

        private void LogResponse(AddBasketStepInput input, AddBasketResponse response, bool isInStock) {
            var status = isInStock ? "In Stock" : "Not in Stock";

            logger.LogInformation(
                input.Job.ToEventId(),
                $@"--[AddBasketStep] Status [{status}] {input.SelectedItem.ToString()}"
            );
        }
    }
}
