using System.Collections.Generic;
using System.Net;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutQueueRequest {
        public string Slug { get; }
        public IEnumerable<Cookie> Cookies { get; }

        public CheckoutQueueRequest(
            string slug,
            IEnumerable<Cookie> cookies
        ) {
            Slug = slug;
            Cookies = cookies;
        }
    }
}
