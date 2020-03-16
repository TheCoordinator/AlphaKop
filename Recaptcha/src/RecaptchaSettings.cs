#nullable disable

namespace AlphaKop.Recaptcha {
    public sealed class RecaptchaSettings {
        public bool Enabled { get; set; }
        public string SecretKey { get; set; }
        public string SiteKey { get; set; }
    }
}