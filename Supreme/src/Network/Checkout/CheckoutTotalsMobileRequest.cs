using AlphaKop.Core.Models.User;
using System.Collections.Generic;
using System.Net;

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