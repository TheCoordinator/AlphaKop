using System;
using System.Threading.Tasks;
using AlphaKop.Core;
using AlphaKop.Core.Flows;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface ISupremeStartStep : ITaskStep<InitialStepInput> { }

    public sealed class SupremeStartStep : ISupremeStartStep {
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

        public async Task Execute(InitialStepInput input) {
            await provider.CreateStep<InitialStepInput, IFetchItemStep>()
                .Execute(input);
        }
    }
}