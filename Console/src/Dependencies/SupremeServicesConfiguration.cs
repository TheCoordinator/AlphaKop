using System;
using System.Net;
using System.Net.Http;
using AlphaKop.Core.Http;
using AlphaKop.Supreme.Config;
using AlphaKop.Supreme.Flows;
using AlphaKop.Supreme.Network;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AlphaKop {
    public class SupremeServicesConfiguration {
        private readonly HostBuilderContext context;
        private readonly IServiceCollection services;

        public SupremeServicesConfiguration(
            HostBuilderContext context,
            IServiceCollection services
        ) {
            this.context = context;
            this.services = services;
        }

        public void ConfigureServices() {
            ConfigureConfigurations();
            ConfigureHttpClients();
            ConfigureRepositories();
            ConfigureFlows();
        }

        private void ConfigureConfigurations() {
            var config = context.Configuration;
            services.Configure<SupremeConfig>(config.GetSection("Supreme"));
        }

        private void ConfigureHttpClients() {
            services.AddHttpClient("supreme_stock", (provider, client) => {
                var config = provider.GetRequiredService<IOptions<SupremeConfig>>().Value;
                client.BaseAddress = new Uri(config.SupremeBaseUrl);
                ConfigureDefaultHttpClient(client, config);
            })
            .ConfigurePrimaryHttpMessageHandler(provider => ConfigurePrimaryHttpHandler(provider: provider, useCookies: false));

            services.AddHttpClient("supreme_checkout", (provider, client) => {
                var config = provider.GetRequiredService<IOptions<SupremeConfig>>().Value;
                client.BaseAddress = new Uri(config.SupremeBaseUrl);
                ConfigureDefaultHttpClient(client, config);
            })
            .ConfigurePrimaryHttpMessageHandler(provider => ConfigurePrimaryHttpHandler(provider: provider, useCookies: true));

            services.AddHttpClient("pooky", (provider, client) => {
                var config = provider.GetRequiredService<IOptions<SupremeConfig>>().Value;
                client.BaseAddress = new Uri(config.PookyBaseUrl);
                ConfigureDefaultHttpClient(client, config);

                client.DefaultRequestHeaders.Add(
                    name: "Auth",
                    value: config.PookyAuthentication
                );
            })
            .ConfigurePrimaryHttpMessageHandler(provider => ConfigurePrimaryHttpHandler(provider: provider, useCookies: false));
        }

        private void ConfigureDefaultHttpClient(HttpClient client, SupremeConfig config) {
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Add(
                name: HttpRequestHeader.UserAgent.ToString(),
                value: config.UserAgent
            );

            client.DefaultRequestHeaders.Add(
                name: HttpRequestHeader.CacheControl.ToString(),
                value: "no-cache"
            );
        }

        private HttpMessageHandler ConfigurePrimaryHttpHandler(IServiceProvider provider, bool useCookies) {
            return new LoggingHandler(
                innerHandler: new HttpClientHandler() {
                    UseCookies = useCookies
                },
                logger: provider.GetService<ILogger<LoggingHandler>>()
            );
        }

        private void ConfigureRepositories() {
            services.AddTransient<ISupremeRequestsFactory, SupremeRequestsFactory>();
            services.AddTransient<IPookyRequestsFactory, PookyRequestsFactory>();

            services.AddSingleton<ISupremeStockRepository, SupremeStockRepository>();
            services.AddScoped<ISupremeCheckoutRepository, SupremeCheckoutRepository>();
            services.AddSingleton<IPookyRepository, PookyRepository>();
        }

        private void ConfigureFlows() {
            services.AddTransient<ISupremeStartStep, SupremeStartStep>();
            services.AddTransient<IFetchItemStep, FetchItemStep>();
            services.AddTransient<IFetchItemDetailsStep, FetchItemDetailsStep>();
            services.AddTransient<IFetchPookyStep, FetchPookyStep>();
            services.AddTransient<IAddBasketStep, AddBasketStep>();
            services.AddTransient<IFetchPookyTicketStep, FetchPookyTicketStep>();
            services.AddTransient<ICaptchaStep, CaptchaStep>();
            services.AddTransient<IFetchCard3DSecureStep, FetchCard3DSecureStep>();
            services.AddTransient<ICheckoutStep, CheckoutStep>();
            services.AddTransient<ICheckoutQueueStep, CheckoutQueueStep>();
            services.AddTransient<ISupremeSuccessStep, SupremeSuccessStep>();
        }
    }
}