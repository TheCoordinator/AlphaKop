using System;
using System.Threading.Tasks;
using AlphaKop.Core;
using AlphaKop.Core.Flows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface ISupremeStartStep : ITaskStep<SupremeJob, SupremeJob> { }

    sealed class SupremeStartStep : ISupremeStartStep {        
        private readonly IServiceProvider provider;
        private readonly ILogger<SupremeStartStep> logger;

        public SupremeJob? Job { get; set; }
        
        public SupremeStartStep(
            IServiceProvider provider,
            ILogger<SupremeStartStep> logger
        ) {
            this.provider = provider;
            this.logger = logger;
        }

        public async Task Execute(SupremeJob parameter) {
            if (Job == null) {
                throw new ArgumentNullException("Job");
            }

            await provider.CreateFetchItemStep(Job.Value)
                .Execute(Unit.Empty);
        }
    }
}