using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AlphaKop.Recaptcha.Services {
    public sealed class RecaptchaService : IRecaptchaService {
        private readonly HttpClient client;
        private readonly RecaptchaSettings settings;

        public RecaptchaService(RecaptchaSettings settings) {
            this.settings = settings;
            this.client = new HttpClient();
        }

        public RecaptchaService(IOptions<RecaptchaSettings> settings) {
            this.settings = settings.Value;
            this.client = new HttpClient();
        }

        public RecaptchaService(IOptions<RecaptchaSettings> settings, HttpClient client) {
            this.settings = settings.Value;
            this.client = client;
        }

        public RecaptchaService(RecaptchaSettings settings, HttpClient client) {
            this.settings = settings;
            this.client = client;
        }

        public async Task<RecaptchaResponse> Validate(HttpRequest request) {
            if (!request.Form.ContainsKey("g-recaptcha-response")) // error if no reason to do anything, this is to alert developers they are calling it without reason.
                throw new ValidationException("Google recaptcha response not found in form. Did you forget to include it?");

            var response = request.Form["g-recaptcha-response"];
            var result = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={settings.SecretKey}&response={response}");
            
            var captchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(result);

            return captchaResponse;
        }
    }
}
