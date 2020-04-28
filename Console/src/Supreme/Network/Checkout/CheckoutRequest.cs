using System.Collections.Generic;
using System.Net;
using AlphaKop.Core.Captcha.Network;
using AlphaKop.Core.Models.User;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutRequest : ICheckoutRequest {
        public string ItemId { get; }
        public string SizeId { get; }
        public string StyleId { get; }
        public int Quantity { get; }
        public IEnumerable<Cookie> Cookies { get; }
        public Pooky Pooky { get; }
        public Captcha Captcha { get; }
        public string? CardinalId { get; }
        public UserProfile Profile { get; }

        public CheckoutRequest(
            string itemId,
            string sizeId,
            string styleId,
            int quantity,
            IEnumerable<Cookie> cookies,
            Pooky pooky,
            Captcha captcha,
            string? cardinalId,
            UserProfile profile
        ) {
            ItemId = itemId;
            SizeId = sizeId;
            StyleId = styleId;
            Quantity = quantity;
            Cookies = cookies;
            Pooky = pooky;
            Captcha = captcha;
            CardinalId = cardinalId;
            Profile = profile;
        }
    }
}
