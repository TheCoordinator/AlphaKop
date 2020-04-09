using System.Collections.Generic;
using System.Net;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutResponse {
        public CheckoutResponseStatus Status { get; }
        public IEnumerable<Cookie> ResponseCookies { get; }

        public CheckoutResponse(
            CheckoutResponseStatus status,
            IEnumerable<Cookie> responseCookies
        ) {
            Status = status;
            ResponseCookies = responseCookies;
        }
    }
}
