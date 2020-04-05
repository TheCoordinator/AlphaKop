using System.Collections.Generic;
using System.Linq;
using System.Net;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Network.Extensions {
    static class PookyCookiesExtensions {
        public static string ToAddToCartCookiesString(this PookyCookies pookyCookies) {
            var cookies = new List<IEnumerable<Cookie>>() { 
                pookyCookies.StaticCookies, pookyCookies.AddToCartCookies 
            };
            
            var cookiesStringArray = cookies
                .SelectMany(cookie => cookie)
                .Select(cookie => cookie.ToString());
                
            return string.Join("; ", cookiesStringArray);
        }
    }
}
