using System;
using System.Threading.Tasks;
using AlphaKop.Core;
using AlphaKop.Core.Flows;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface ISupremeSuccessStep : ITaskStep<SuccessStepInput> { }

    public sealed class SupremeSuccessStep : ISupremeSuccessStep {
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public int Retries { get; set; }

        public SupremeSuccessStep(
            IServiceProvider provider,
            ILogger<SupremeSuccessStep> logger
        ) {
            this.provider = provider;
            this.logger = logger;
        }

        public async Task Execute(SuccessStepInput input) {
            var sale = input.CheckoutResponse.PurchaseSale;

            logger.LogInformation(
                input.Job.ToEventId(),
                $@"--Success 🎉🚀🔥 Status [{input.CheckoutResponse.Status}] {input.SelectedItem.ToString()} Total Cost [{sale?.Currency} {sale?.TotalCartCost}]"
            );

            await Task.Delay(100);
        }
    }
}