using System;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    abstract class BaseStep<TParameter> : ITaskStep<TParameter, SupremeJob> {
        protected readonly IServiceProvider provider;

        public SupremeJob? Job { get; set; }
        public int Retries { get; set; }

        protected EventId JobEventId {
            get {
                return Job?.ToEventId() ?? new EventId(id: 0);
            }
        }

        public BaseStep(
            IServiceProvider provider
        ) {
            this.provider = provider;
        }

        public async Task Execute(TParameter parameter) {
            if (Job == null) {
                throw new ArgumentNullException("Job");
            }

            await Task.Delay(Job.Value.StartDelay);

            await Execute(parameter: parameter, job: Job.Value);
        }

        protected abstract Task Execute(TParameter parameter, SupremeJob job);
    }
}
