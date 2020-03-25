using System;
using System.Threading.Tasks;
using AlphaKop.Core;
using AlphaKop.Core.Flows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface ISupremeStartStep : ITaskStep<SupremeJob, SupremeJob> { }

    sealed class SupremeStartStep : BaseStep<SupremeJob>, ISupremeStartStep {        
        private readonly ILogger<SupremeStartStep> logger;
        
        public SupremeStartStep(
            IServiceProvider provider,
            ILogger<SupremeStartStep> logger
        ) : base(provider) {
            this.logger = logger;
        }

        protected override async Task Execute(SupremeJob parameter, SupremeJob job) {
            await provider.CreateFetchItemStep(job)
                .Execute(Unit.Empty);
        }
    }
}