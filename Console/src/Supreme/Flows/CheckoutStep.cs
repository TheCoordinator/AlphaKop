using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Repositories;
using AlphaKop.Supreme.Network;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface ICheckoutStep : ITaskStep<CheckoutStepParameter, SupremeJob> { }

    sealed class CheckoutStep : BaseStep<CheckoutStepParameter>, ICheckoutStep {
        private const int maxRetries = 10;
        private readonly ISupremeRepository supremeRepository;
        private readonly ILogger<CheckoutStep> logger;

        public CheckoutStep(
            ISupremeRepository supremeRepository,
            IServiceProvider provider,
            ILogger<CheckoutStep> logger
        ) : base(provider) {
            this.supremeRepository = supremeRepository;
            this.logger = logger;
        }

        protected override async Task Execute(CheckoutStepParameter parameter, SupremeJob job) {
            try {
                if (Retries >= maxRetries) {
                    await provider.CreateFetchItemDetailsStep(job)
                        .Execute(parameter.SelectedItem.Item);

                    return;
                }

                var request = new CheckoutRequest(
                    itemId: parameter.SelectedItem.Item.Id,
                    sizeId: parameter.SelectedItem.Size.Id,
                    styleId: parameter.SelectedItem.Style.Id,
                    quantity: 1,
                    basketResponse: parameter.BasketResponse,
                    pooky: parameter.Pooky,
                    pookyTicket: parameter.PookyTicket,
                    captcha: parameter.Captcha,
                    profile: job.Profile
                );

                await supremeRepository.Checkout(request);

                // logger.LogInformation(
                //     JobEventId,
                //     $"Add Basket Response Item: {request.ItemId} Style: {request.StyleId}, Size: {request.SizeId}\n" +
                //     string.Join("\n", response.ItemSizesStock.Select(r => r.ToString()))
                // );

                // if (response.ItemSizesStock.Any(r => r.InStock == true) && response.Ticket != null) {
                //     var pookyTicketParam = new PookyTicketStepParameter(
                //         selectedItem: parameter.SelectedItem,
                //         basketResponse: response,
                //         basketTicket: response.Ticket,
                //         pooky: parameter.Pooky
                //     );

                //     await provider.CreateFetchPookyTicketStep(job)
                //         .Execute(pookyTicketParam);
                // } else {
                //     await provider.CreateCheckoutStep(job, Retries + 1)
                //         .Execute(parameter);
                // }
            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "Failed to retrieve Checkout");

                await provider.CreateCheckoutStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }
    }
}
