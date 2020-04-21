using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AlphaKop.Supreme.Flows {
    public struct CheckoutCookies {
        public IEnumerable<IEnumerable<Cookie>> Cookies { get; }

        public IEnumerable<Cookie> CookiesList {
            get { return Cookies.SelectMany(cookie => cookie); }
        }

        public CheckoutCookies(IEnumerable<IEnumerable<Cookie>> cookies) {
            Cookies = cookies;
        }
    }
}