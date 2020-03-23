using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlphaKop.Core;
using AlphaKop.Core.Models.User;
using AlphaKop.Supreme.Config;
using AlphaKop.Supreme.Flows;
using AlphaKop.Supreme.Repositories;
using AlphaKop.Supreme.Requests;
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

            ConfigureRepositories(services);

            ConfigureApplication(services);

            return services;
        }

        private static void ConfigureRepositories(IServiceCollection services) {
            services.AddSingleton<IPookyRepository, PookyRepository>();
            services.AddSingleton<ISupremeRepository, SupremeRepository>();
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
                logging.AddFile("AlphaKop");
            });
        }

        private static void ConfigureServicesConfig(
            IServiceCollection services,
            IConfigurationRoot config
        ) {
            services.Configure<SupremeConfig>(config.GetSection("Supreme"));
        }

        private static IConfigurationRoot GetConfiguration() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }
    }
}
