using AlphaKop.Core.Captcha.Config;
using AlphaKop.Core.Captcha.Repositories;
using AlphaKop.Core.CreditCard;
using AlphaKop.Core.Services.TextMatching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        private void ConfigureDefaultServices() {
            services.AddTransient<ITextMatching, TextMatching>();
            services.AddTransient<ICreditCardValidator, DefaultCreditCardValidator>();
            services.AddTransient<ICreditCardFormatter, CreditCardFormatter>();
        }

        private void ConfigureConfigurations() {
            var config = context.Configuration;
            services.Configure<CaptchaConfig>(config.GetSection("CaptchaResolver"));
        }

        private void ConfigureCaptcha() {
            services.AddSingleton<ICaptchaRepository, CaptchaRepository>();
        }
    }
}