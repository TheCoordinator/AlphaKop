using System;
using AlphaKop.Core.Flows;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaKop.Supreme.Flows {
    static class IServiceProviderExtensions {
        public static TTaskStep CreateStep<TTaskInput, TTaskStep>(this IServiceProvider provider, int retries = 0) 
            where TTaskInput: IStepInput 
            where TTaskStep: ITaskStep<TTaskInput> {
            var step = provider.GetRequiredService<TTaskStep>();
            step.Retries = retries;
            return step;
        }

        public static IAddBasketStep CreateAddBasketStep(
            this IServiceProvider provider,
            SupremeJob job,
            int retries = 0
        ) {
            var step = provider.GetRequiredService<IAddBasketStep>();
            step.Job = job;
            step.Retries = retries;
            return step;
        }

        public static IFetchPookyTicketStep CreateFetchPookyTicketStep(
            this IServiceProvider provider,
            SupremeJob job,
            int retries = 0
        ) {
            var step = provider.GetRequiredService<IFetchPookyTicketStep>();
            step.Job = job;
            step.Retries = retries;
            return step;
        }

        public static ICaptchaStep CreateCaptchaStep(
            this IServiceProvider provider,
            SupremeJob job,
            int retries = 0
        ) {
            var step = provider.GetRequiredService<ICaptchaStep>();
            step.Job = job;
            step.Retries = retries;
            return step;
        }

        public static ICheckoutStep CreateCheckoutStep(
            this IServiceProvider provider,
            SupremeJob job,
            int retries = 0
        ) {
            var step = provider.GetRequiredService<ICheckoutStep>();
            step.Job = job;
            step.Retries = retries;
            return step;
        }

        public static ICheckoutQueueStep CreateCheckoutQueueStep(
            this IServiceProvider provider,
            SupremeJob job,
            int retries = 0
        ) {
            var step = provider.GetRequiredService<ICheckoutQueueStep>();
            step.Job = job;
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