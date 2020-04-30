using System.Collections.Generic;
using System.Net;
using AlphaKop.Core.Models.User;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutTotalsMobileRequest {
        public string SizeId { get; }
        public int Quantity { get; }
        public IEnumerable<Cookie> Cookies { get; }
        public UserProfile Profile { get; }

        public CheckoutTotalsMobileRequest(
            string sizeId,
            int quantity,
            IEnumerable<Cookie> cookies,
            UserProfile profile
        ) {
            SizeId = sizeId;
            Quantity = quantity;
            Cookies = cookies;
            Profile = profile;
        }
    }
}