using System;
using AlphaKop.Core.Flows;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaKop.Supreme.Flows {
    static class IServiceProviderExtensions {
        public static TTaskStep CreateStep<TTaskInput, TTaskStep>(this IServiceProvider provider, int retries = 0)
            where TTaskInput : IStepInput
            where TTaskStep : ITaskStep<TTaskInput> {
            var step = provider.GetRequiredService<TTaskStep>();
            step.Retries = retries;
            return step;
        }

        public static ISupremeSuccessStep CreateSuccessStep(
            this IServiceProvider provider,
            SupremeJob job
        ) {
            var step = provider.GetRequiredService<ISupremeSuccessStep>();
            step.Job = job;
            return step;
        }
    }
}