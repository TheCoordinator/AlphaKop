using AlphaKop.Core.Flows;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AlphaKop.Supreme.Flows {
    static class IServiceProviderExtensions {
        public static TTaskStep CreateStep<TTaskInput, TTaskStep>(this IServiceProvider provider, int retries = 0)
            where TTaskInput : IStepInput
            where TTaskStep : ITaskStep<TTaskInput> {
            var step = provider.GetRequiredService<TTaskStep>();
            step.Retries = retries;
            return step;
        }
    }
}