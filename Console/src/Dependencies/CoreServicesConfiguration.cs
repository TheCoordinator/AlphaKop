using AlphaKop.Core.Captcha.Config;
using AlphaKop.Core.Captcha.Network;
using AlphaKop.Core.Captcha.Repositories;
using AlphaKop.Core.CreditCard;
using AlphaKop.Core.Services.TextMatching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

namespace AlphaKop {
    public sealed class CoreServicesConfiguration {
        private readonly HostBuilderContext context;
        private readonly IServiceCollection services;

        public CoreServicesConfiguration(HostBuilderContext context, IServiceCollection services) {
            this.context = context;
            this.services = services;
        }

        public void ConfigureServices() {
            ConfigureConfigurations();
            ConfigureDefaultServices();
            ConfigureCaptcha();
        }

        private void ConfigureConfigurations() {
            var config = context.Configuration;
            services.Configure<CaptchaConfig>(config.GetSection("CaptchaResolver"));
        }

        private void ConfigureDefaultServices() {
            services.AddTransient<ITextMatching, TextMatching>();
            services.AddTransient<ICreditCardValidator, DefaultCreditCardValidator>();
            services.AddTransient<ICreditCardFormatter, CreditCardFormatter>();
        }

        private void ConfigureCaptcha() {
            services.AddHttpClient("captcha", (provider, client) => {
                var config = provider.GetRequiredService<IOptions<CaptchaConfig>>().Value;
                client.BaseAddress = new Uri(config.BaseUrl);
            });

            services.AddTransient<ICaptchaRequestsFactory, CaptchaRequestsFactory>();
            services.AddSingleton<ICaptchaRepository, CaptchaRepository>();
        }
    }
}