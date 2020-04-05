using System.Collections.Generic;
using System.Linq;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Network.Extensions {
    static class PookyCookiesExtensions {
        public static string ToAddToCartCookiesString(this PookyCookies pookyCookies) {
            var cookies = ToAddToCartCookies(pookyCookies);
            var cookiesStringArray = cookies
                .Select(pair => $"{pair.Key}={pair.Value}");

            return string.Join("; ", cookiesStringArray);
        }

        public static IDictionary<string, string> ToAddToCartCookies(this PookyCookies pookyCookies) {
            IDictionary<string, string>[] dictionaries = { 
                pookyCookies.AddToCartCookies, pookyCookies.StaticCookies 
            };

            return dictionaries
                .SelectMany(dict => dict)
                .ToLookup(pair => pair.Key, pair => pair.Value)
                .ToDictionary(group => group.Key, group => group.FirstOrDefault());
        }
    }
}
