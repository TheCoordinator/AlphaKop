using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AlphaKop.CaptchaResolverApp {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
