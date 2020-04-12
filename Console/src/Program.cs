using System;
using System.Net.Http;
using System.IO;
using System.Threading;
using AlphaKop.ConsoleApp;
using AlphaKop.Core.Captcha.Config;
using AlphaKop.Core.Captcha.Repositories;
using AlphaKop.Core.CreditCard;
using AlphaKop.Core.Services.TextMatching;
using AlphaKop.Supreme.Config;
using AlphaKop.Supreme.Flows;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace AlphaKop {
    class Program {
        private static IHost? host;
        private static ManualResetEvent quitEvent = new ManualResetEvent(false);

        static void Main(string[] args) {
            Console.CancelKeyPress += (sender, eArgs) => {
                quitEvent.Set();
                eArgs.Cancel = true;
            };

            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration(ConfigureConfiguration)
                .ConfigureLogging(ConfigureLogging)
                .ConfigureServices(ConfigureServicesConfig)
                .ConfigureServices(ConfigureCoreServices)
                .ConfigureServices(ConfigureSupremeServices)
                .ConfigureServices(ConfigureApplication);

            host = hostBuilder.Build();

            var application = host.Services.GetService<ConsoleApplication>();
            application.CsvTaskPath = args[0];

            application.Run();

            quitEvent.WaitOne();
        }

        #region AppConfiguration

        private static void ConfigureConfiguration(IConfigurationBuilder builder) {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
        }

        private static void ConfigureServicesConfig(
            HostBuilderContext context,
            IServiceCollection services
        ) {
            var config = context.Configuration;

            services.Configure<SupremeConfig>(config.GetSection("Supreme"));
            services.Configure<CaptchaConfig>(config.GetSection("CaptchaResolver"));
        }

        private static void ConfigureLogging(ILoggingBuilder builder) {
            builder.AddConsole();
            builder.AddFile();
        }

        #endregion

        #region Core

        private static void ConfigureCoreServices(HostBuilderContext context, IServiceCollection services) {
            services.AddTransient<ITextMatching, TextMatching>();
            services.AddTransient<ICreditCardValidator, DefaultCreditCardValidator>();
            services.AddTransient<ICreditCardFormatter, CreditCardFormatter>();
            services.AddSingleton<ICaptchaRepository, CaptchaRepository>();

            // TODO: Captcha HttpClient.
        }

        #endregion

        #region Console

        private static void ConfigureApplication(
            HostBuilderContext context,
            IServiceCollection services
        ) {
            services.AddSingleton<ConsoleApplication>();
        }

        #endregion

        #region Supreme

        private static void ConfigureSupremeServices(
            HostBuilderContext context,
            IServiceCollection services
        ) {
            ConfigureSupremeRepositories(services);
            ConfigureSupremeFlows(services);
        }

        private static void ConfigureSupremeRepositories(IServiceCollection services) {
            services.AddSingleton<ISupremeRepository, SupremeRepository>();
            services.AddSingleton<IPookyRepository, PookyRepository>();
        }

        private static void ConfigureSupremeFlows(IServiceCollection services) {
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

        #endregion
    }
}
