using System;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaKop.Supreme.Flows {
    static class IServiceProviderExtensions {
        public static IFetchItemStep CreateFetchItemStep(
            this IServiceProvider provider,
            SupremeJob job
        ) {
            var step = provider.GetRequiredService<IFetchItemStep>();
            step.Job = job;
            return step;
        }

        public static IFetchItemDetailsStep CreateFetchItemDetailsStep(
            this IServiceProvider provider,
            SupremeJob job
        ) {
            var step = provider.GetRequiredService<IFetchItemDetailsStep>();
            step.Job = job;
            return step;
        }
    }
}