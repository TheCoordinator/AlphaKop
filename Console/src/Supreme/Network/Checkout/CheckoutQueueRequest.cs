using System.Collections.Generic;
using System.Net;
using AlphaKop.Core.Captcha.Network;
using AlphaKop.Core.Models.User;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutQueueRequest : ICheckoutRequest {
        public string ItemId { get; }
        public string SizeId { get; }
        public string StyleId { get; }
        public int Quantity { get; }
        public string Slug { get; }
        public IEnumerable<Cookie> Cookies { get; }
        public Pooky Pooky { get; }
        public Captcha Captcha { get; }
        public UserProfile Profile { get; }

        public CheckoutQueueRequest(
            string itemId,
            string sizeId,
            string styleId,
            int quantity,
            string slug,
            IEnumerable<Cookie> cookies,
            Pooky pooky,
            Captcha captcha,
            UserProfile profile
        ) {
            ItemId = itemId;
            SizeId = sizeId;
            StyleId = styleId;
            Quantity = quantity;
            Slug = slug;
            Cookies = cookies;
            Pooky = pooky;
            Captcha = captcha;
            Profile = profile;
        }
    }
}
