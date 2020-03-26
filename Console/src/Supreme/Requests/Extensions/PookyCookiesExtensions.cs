using System.Collections.Generic;
using System.Linq;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Requests.Extensions {
    static class PookyCookiesExtensions {
        public static string ToAddToCartCookiesString(this Pooky pooky) {
            var cookies = ToAddToCartCookies(pooky);
            var cookiesStringArray = cookies
                .Select(pair => $"{pair.Key}={pair.Value}");

            return string.Join("; ", cookiesStringArray);
        }

        public static Dictionary<string, string> ToAddToCartCookies(this Pooky pooky) {
            var addToCart = pooky.Cookies.AddToCart.ToCookies();
            var staticData = pooky.Cookies.StaticData.ToCookies();

            Dictionary<string, string>[] dictionaries = { addToCart, staticData };

            return dictionaries
                .SelectMany(dict => dict)
                .ToLookup(pair => pair.Key, pair => pair.Value)
                .ToDictionary(group => group.Key, group => group.FirstOrDefault());
        }
    }
}
