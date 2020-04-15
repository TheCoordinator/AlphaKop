using System;
using System.Threading.Tasks;
using AlphaKop.Core;
using AlphaKop.Core.Flows;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface ISupremeSuccessStep : ITaskStep<SuccessStepParameter, SupremeJob> { }

    sealed class SupremeSuccessStep : BaseStep<SuccessStepParameter>, ISupremeSuccessStep {        
        private readonly ILogger logger;
        
        public SupremeSuccessStep(
            IServiceProvider provider,
            ILogger<SupremeSuccessStep> logger
        ) : base(provider) {
            this.logger = logger;
        }

        protected override async Task Execute(SuccessStepParameter parameter, SupremeJob job) {
            var sale = parameter.CheckoutResponse.Status.PurchaseSale;

            logger.LogInformation(
                JobEventId,
                $@"--Success {parameter.SelectedItem.ToString()} Total Cost [{sale?.Currency} {sale?.TotalCartCost}]"
            );

            await Task.Delay(1);
        }
    }
}