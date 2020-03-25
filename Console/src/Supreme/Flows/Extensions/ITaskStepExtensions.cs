using System;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaKop.Supreme.Flows {
    static class IServiceProviderExtensions {
        public static IFetchItemStep CreateFetchItemStep(
            this IServiceProvider provider,
            SupremeJob job,
            int retries = 0
        ) {
            var step = provider.GetRequiredService<IFetchItemStep>();
            step.Job = job;
            step.Retries = retries;
            return step;
        }

        public static IFetchItemDetailsStep CreateFetchItemDetailsStep(
            this IServiceProvider provider,
            SupremeJob job,
            int retries = 0
        ) {
            var step = provider.GetRequiredService<IFetchItemDetailsStep>();
            step.Job = job;
            step.Retries = retries;
            return step;
        }
    }
}