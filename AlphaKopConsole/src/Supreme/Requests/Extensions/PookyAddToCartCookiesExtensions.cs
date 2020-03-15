using System.Collections.Generic;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Requests.Extensions {
    static class PookyAddToCartCookiesExtensions {
        public static Dictionary<string, string> ToCookies(this PookyAddToCart addToCart) {
            return new Dictionary<string, string>() {
                { "pooky_settings", addToCart.PookySettings },
                { "updated_pooky_coherence", addToCart.UpdatedPookyCoherence },
                { "pooky_data", addToCart.PookyData },
                { "pooky_recaptcha_coherence", addToCart.PookyRecaptchaCoherence },
                { "pooky_telemetry", addToCart.PookyTelemetry },
                { "pooky_electric", addToCart.PookyElectric },
                { "pooky_mouse", addToCart.PookyMouse },
                { "pooky_performance", addToCart.PookyPerformance },
                { "pooky_recaptcha", addToCart.PookyPerformance },
            };
        }
    }
}