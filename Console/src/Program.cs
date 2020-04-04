using System;
using System.IO;
using System.Threading;
using AlphaKop.Core.Captcha.Config;
using AlphaKop.Core.Captcha.Repositories;
using AlphaKop.Core.Services.TextMatching;
using AlphaKop.Supreme.Config;
using AlphaKop.Supreme.Flows;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AlphaKop {
    class Program {
        private static ManualResetEvent quitEvent = new ManualResetEvent(false);
        private static IServiceCollection? services;
        private static IConfigurationRoot? config;
        private static ILogger<Program>? logger;

        static void Main(string[] args) {
            Console.CancelKeyPress += (sender, eArgs) => {
                logger.LogDebug("Stopping the application");
                quitEvent.Set();
                eArgs.Cancel = true;
            };

            config = GetConfiguration();
            services = ConfigureServices(config);

            var serviceProvider = services.BuildServiceProvider();
            logger = serviceProvider.GetService<ILogger<Program>>();

            var application = serviceProvider.GetService<ConsoleApplication>();

            application.Run();

            quitEvent.WaitOne();

            logger.LogDebug("Stopped the application");
        }

        private static IServiceCollection ConfigureServices(IConfigurationRoot config) {
            IServiceCollection services = new ServiceCollection();

            ConfigureLogging(services, config);
            ConfigureServicesConfig(services, config);

            ConfigureCore(services);
            ConfigureSupreme(services);
            ConfigureApplication(services);

            return services;
        }

        private static void ConfigureCore(IServiceCollection services) {
            services.AddTransient<ITextMatching, TextMatching>();
            services.AddSingleton<ICaptchaRepository, CaptchaRepository>();
        }

        private static void ConfigureSupreme(IServiceCollection services) {
            ConfigureSupremeRepositories(services);
            ConfigureSupremeFlows(services);
        }

        private static void ConfigureSupremeRepositories(IServiceCollection services) {
            services.AddSingleton<IPookyRepository, PookyRepository>();
            services.AddSingleton<ISupremeRepository, SupremeRepository>();
        }

        private static void ConfigureSupremeFlows(IServiceCollection services) {
            services.AddTransient<ISupremeStartStep, SupremeStartStep>();
            services.AddTransient<IFetchItemStep, FetchItemStep>();
            services.AddTransient<IFetchItemDetailsStep, FetchItemDetailsStep>();
            services.AddTransient<IFetchPookyStep, FetchPookyStep>();
            services.AddTransient<IAddBasketStep, AddBasketStep>();
            services.AddTransient<ICaptchaStep, CaptchaStep>();
        }

        private static void ConfigureApplication(IServiceCollection services) {
            services.AddSingleton<ConsoleApplication>();
        }

        private static void ConfigureLogging(
            IServiceCollection services,
            IConfigurationRoot config
        ) {
            services.AddLogging(logging => {
                logging.AddConfiguration(config.GetSection("Logging"));
                logging.AddConsole();
                logging.AddFile();
            });
        }

        private static void ConfigureServicesConfig(
            IServiceCollection services,
            IConfigurationRoot config
        ) {
            services.Configure<SupremeConfig>(config.GetSection("Supreme"));
            services.Configure<CaptchaConfig>(config.GetSection("CaptchaResolver"));
        }

        private static IConfigurationRoot GetConfiguration() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }
    }
}
