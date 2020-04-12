using System;
using System.IO;
using System.Threading;
using AlphaKop.ConsoleApp;
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
                .ConfigureServices(ConfigureCoreServices)
                .ConfigureServices(ConfigureSupremeServices)
                .ConfigureServices(ConfigureApplication);

            host = hostBuilder.Build();

            var application = host.Services.GetService<ConsoleApplication>();
            application.CsvTaskPath = args[0];

            application.Run();

            quitEvent.WaitOne();
        }

        private static void ConfigureConfiguration(IConfigurationBuilder builder) {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
        }

        private static void ConfigureLogging(ILoggingBuilder builder) {
            builder.AddConsole();
            builder.AddFile();
        }

        private static void ConfigureApplication(HostBuilderContext context, IServiceCollection services) {
            services.AddSingleton<ConsoleApplication>();
        }

        private static void ConfigureCoreServices(HostBuilderContext context, IServiceCollection services) {
            new CoreServicesConfiguration(context, services)
                .ConfigureServices();
        }

        private static void ConfigureSupremeServices(HostBuilderContext context, IServiceCollection services) {
            new SupremeServicesConfiguration(context, services)
                .ConfigureServices();
        }
    }
}
