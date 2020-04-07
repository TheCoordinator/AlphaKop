using System.Collections.Generic;
using System.Linq;
using System.Net;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Network.Extensions {
    static class CookiesExtensions {
        public static string ToCookiesString(this IEnumerable<IEnumerable<Cookie>> cookies) {
            var cookiesStringArray = cookies
                .SelectMany(cookie => cookie)
                .Select(cookie => cookie.ToString());

            return string.Join("; ", cookiesStringArray);
        }
    }
}
