using System;
using System.Net;
using System.Net.Http;
using AlphaKop.Supreme.Config;
using AlphaKop.Supreme.Flows;
using AlphaKop.Supreme.Network;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddHttpClient("supreme", (provider, client) => {
                var config = provider.GetRequiredService<IOptions<SupremeConfig>>().Value;
                client.BaseAddress = new Uri(config.SupremeBaseUrl);
                ConfigureDefaultHttpClient(client, config);
            })
            .ConfigurePrimaryHttpMessageHandler(ConfigurePrimaryHttpHandler);

            services.AddHttpClient("pooky", (provider, client) => {
                var config = provider.GetRequiredService<IOptions<SupremeConfig>>().Value;
                client.BaseAddress = new Uri(config.PookyBaseUrl);
                ConfigureDefaultHttpClient(client, config);

                client.DefaultRequestHeaders.Add(
                    name: "Auth",
                    value: config.PookyAuthentication
                );
            })
            .ConfigurePrimaryHttpMessageHandler(ConfigurePrimaryHttpHandler);
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

        private HttpMessageHandler ConfigurePrimaryHttpHandler() {
            return new HttpClientHandler() {
                UseCookies = false
            };
        }

        private void ConfigureRepositories() {
            services.AddTransient<ISupremeRequestsFactory, SupremeRequestsFactory>();
            services.AddTransient<IPookyRequestsFactory, PookyRequestsFactory>();

            services.AddSingleton<ISupremeRepository, SupremeRepository>();
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
            services.AddTransient<ICheckoutStep, CheckoutStep>();
            services.AddTransient<ICheckoutQueueStep, CheckoutQueueStep>();
            services.AddTransient<ISupremeSuccessStep, SupremeSuccessStep>();
        }
    }
}