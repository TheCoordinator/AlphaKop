using AlphaKop.Core.Flows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
            using var scope = provider.CreateScope();

            await scope.ServiceProvider.CreateStep<InitialStepInput, IFetchItemStep>()
                .Execute(input);

            logger.LogDebug(input.Job.ToEventId(), "Task scope disposed");
        }
    }
}