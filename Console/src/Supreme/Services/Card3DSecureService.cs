using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AlphaKop.Core.Retry;
using AlphaKop.Supreme.Config;
using Microsoft.Extensions.Options;
using PuppeteerSharp;
using PuppeteerSharp.Mobile;

namespace AlphaKop.Supreme.Services {
    public sealed class Card3DSecureService : ICard3DSecureService {
        private bool isBrowserBinaryFetched = false;
        private readonly SupremeConfig config;

        public Card3DSecureService(IOptions<SupremeConfig> config) {
            this.config = config.Value;
        }

        public async Task<Card3DSecureResponse> FetchCard3DSecure(string htmlContent) {
            var fileContent = await GetHtmlFile();

            if (fileContent == null) {
                throw new NullReferenceException("Html file not found");
            }

            var htmlFileContent = fileContent.Replace("[HTML_CONTENT]", htmlContent);
            return await FetchResponse(htmlFileContent);
        }

        private async Task<Card3DSecureResponse> FetchResponse(string htmlContent) {
            await FetchBrowserBinaryIfNeeded();

            using (var browser = await CreateBrowser())
            using (var page = await browser.NewPageAsync()) {
                await ConfigurePage(page);
                await page.GoToAsync(config.SupremeMobileWebsite);
                await page.SetContentAsync(htmlContent);

                var frame = GetCardinalFrame(page);

                var element = await frame.WaitForSelectorAsync("#referenceId", new WaitForSelectorOptions() {
                    Timeout = 2_000
                });

                var property = await element.GetPropertyAsync("value");
                var cardinalId = property?.RemoteObject?.Value?.ToString();

                if (cardinalId == null) {
                    throw new NullReferenceException("CardinalId cannot be found");
                }

                return new Card3DSecureResponse(cardinalId);
            }
        }

        private Frame GetCardinalFrame(Page page) {
            return Retry.Do<Frame>(
                retryInterval: TimeSpan.FromMilliseconds(100),
                maxAttemptCount: 20,
                () => {
                    return page.Frames.First(frame => frame.Name.ToLower() == "cardinal-collector");
                }
            );
        }

        private async Task ConfigurePage(Page page) {
            await page.SetUserAgentAsync(config.UserAgent);
            await page.EmulateAsync(Puppeteer.Devices[DeviceDescriptorName.IPhoneX]);
            await page.SetBypassCSPAsync(true);
        }

        private async Task<Browser> CreateBrowser() {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions {
                Args = new String[] {
                    "--disable-web-security",
                    "--disable-background-networking",
                    "--disable-background-timer-throttling",
                    "--disable-backgrounding-occluded-windows",
                    "--disable-breakpad",
                    "--disable-client-side-phishing-detection",
                    "--disable-default-apps",
                    "--disable-dev-shm-usage",
                    "--disable-extensions",
                    "--disable-features=site-per-process",
                    "--disable-hang-monitor",
                    "--disable-ipc-flooding-protection",
                    "--disable-popup-blocking",
                    "--disable-prompt-on-repost",
                    "--disable-renderer-backgrounding",
                    "--disable-sync",
                    "--disable-translate",
                    "--metrics-recording-only",
                    "--safebrowsing-disable-auto-update",
                    "--mute-audio",
                    "--disable-gpu",
                    "--enable-accelerated-mjpeg-decode",
                    "--enable-accelerated-video",
                    "--enable-gpu-rasterization",
                    "--no-sandbox",
                    "--disable-setuid-sandbox",
                    "--enable-native-gpu-memory-buffers",
                    "--ignore-gpu-blacklist"
                },
                Headless = false, // TODO: set to true
                Devtools = true,
                IgnoreHTTPSErrors = true
            });

            return browser;
        }

        private async Task FetchBrowserBinaryIfNeeded() {
            if (isBrowserBinaryFetched == true) {
                return;
            }

            var fetcher = new BrowserFetcher();
            await fetcher.DownloadAsync(BrowserFetcher.DefaultRevision);
            isBrowserBinaryFetched = true;
        }

        private async Task<string> GetHtmlFile() {
            var assembly = typeof(Card3DSecureService).GetTypeInfo().Assembly;
            // TODO: Move to Supreme assembly.
            var resourceStream = assembly.GetManifestResourceStream("Console.Supreme.Resources.Card3DSecure.html");

            if (resourceStream == null) {
                throw new NullReferenceException("Cannot find Card3DSecure.html resource.");
            }

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8)) {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
