using System;
using System.Threading.Tasks;
using AlphaKop.Core;
using AlphaKop.Core.Flows;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface ISupremeStartStep : ITaskStep<SupremeJob> { }

    sealed class SupremeStartStep : ISupremeStartStep {
        public int Retries { get; set; }

        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public SupremeStartStep(
            IServiceProvider provider,
            ILogger<SupremeStartStep> logger
        ) {
            this.provider = provider;
            this.logger = logger;
        }

        public async Task Execute(SupremeJob input) {
            await provider.CreateStep<SupremeJob, IFetchItemStep>()
                .Execute(input);
        }
    }
}