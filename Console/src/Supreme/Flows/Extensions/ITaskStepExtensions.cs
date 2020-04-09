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

        public static IFetchPookyStep CreateFetchPookyStep(
            this IServiceProvider provider,
            SupremeJob job,
            int retries = 0
        ) {
            var step = provider.GetRequiredService<IFetchPookyStep>();
            step.Job = job;
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