using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlphaKop.Supreme.Services {
    public sealed class Card3DSecureService: ICard3DSecureService {
        public async Task<string> FetchCardinalId(string htmlContent) {
            var fileContent = await GetHtmlFile();
            throw new NotImplementedException();
        }

        private async Task<string> GetHtmlFile() {
            var assembly = typeof(Card3DSecureService).GetTypeInfo().Assembly;
            // TODO: Move to Supreme assembly.
            var resourceStream = assembly.GetManifestResourceStream("Console.Supreme.Resources.Songbird.html");

            if (resourceStream == null) {
                throw new NullReferenceException("Cannot find Songbird.html resource.");
            }

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8)) {
                return await reader.ReadToEndAsync();
            }
        }

        /*
        private void selenium() {
                        var directory = "/Users/peyman/Projects/Peyman/Bots/AlphaKop/WebDrivers";
            var options = new ChromeOptions();
            
            // options.AddArgument("headless");

            IWebDriver driver = new ChromeDriver(
                chromeDriverDirectory: directory,
                options: options
            );

            var fileUrl = "/Users/peyman/Projects/Peyman/Bots/AlphaKop/Console/src/Supreme/Songbird/Songbird.html";
            driver.Navigate().GoToUrl($"file://{fileUrl}");

            var element = driver.FindElement(by: By.Id("jwt_response"));

            string? response = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(driver => {
                    var response = driver.FindElement(by: By.Id("jwt_response"))
                        .GetAttribute("value");

                    return response == "NO_RESPONSE" ? null : response;
                });
            
            Console.WriteLine($"Response = {response}");
        }
        */
    }
}
