using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AlphaKop.Supreme.Flows {
    public struct CheckoutCookies {
        public IEnumerable<Cookie> Cookies { get; }

        public CheckoutCookies(IEnumerable<Cookie> cookies) {
            Cookies = cookies;
        }

        public CheckoutCookies(IEnumerable<IEnumerable<Cookie>> cookiesList) {
            Cookies = cookiesList
                .SelectMany(cookie => cookie);
        }
    }
}