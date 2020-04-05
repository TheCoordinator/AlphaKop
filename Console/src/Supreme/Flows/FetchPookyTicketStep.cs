using System;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchPookyTicketStep : ITaskStep<PookyTicketStepParameter, SupremeJob> { }

    sealed class FetchPookyTicketStep : BaseStep<PookyTicketStepParameter>, IFetchPookyTicketStep {
        private const int maxRetries = 10;
        private readonly IPookyRepository pookyRepository;
        private readonly ILogger<FetchPookyTicketStep> logger;

        public FetchPookyTicketStep(
            IPookyRepository pookyRepository,
            IServiceProvider provider,
            ILogger<FetchPookyTicketStep> logger
        ) : base(provider) {
            this.pookyRepository = pookyRepository;
            this.logger = logger;
        }

        protected override async Task Execute(PookyTicketStepParameter parameter, SupremeJob job) {
            if (Retries >= maxRetries) {
                await provider.CreateFetchItemDetailsStep(job)
                    .Execute(parameter.SelectedItem.Item);

                return;
            }

            try {
                var pookyRegion = PookyRegionUtil.From(job.Region);
                var pookyTicket = await pookyRepository.FetchPookyTicket(
                    region: pookyRegion,
                    ticket: parameter.BasketTicket
                );

                logger.LogInformation(
                    JobEventId,
                    $"Fetched Pooky Ticket {parameter.SelectedItem.Item.Id}"
                );

                var captchaStepParam = new CaptchaStepParameter(
                    selectedItem: parameter.SelectedItem,
                    basketResponse: parameter.BasketResponse,
                    pooky: parameter.Pooky,
                    pookyTicket: pookyTicket
                );

                await provider.CreateCaptchaStep(job)
                    .Execute(captchaStepParam);

            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "Failed to Fetch Pooky Ticket");

                await provider.CreateFetchPookyTicketStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }
    }
}