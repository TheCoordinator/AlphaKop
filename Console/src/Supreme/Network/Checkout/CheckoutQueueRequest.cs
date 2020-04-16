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
        public AddBasketResponse BasketResponse { get; }
        public CheckoutResponse CheckoutResponse { get; }
        public Pooky Pooky { get; }
        public PookyTicket PookyTicket { get; }
        public Captcha Captcha { get; }
        public UserProfile Profile { get; }

        public CheckoutQueueRequest(
            string itemId,
            string sizeId,
            string styleId,
            int quantity,
            string slug,
            AddBasketResponse basketResponse,
            CheckoutResponse checkoutResponse,
            Pooky pooky,
            PookyTicket pookyTicket,
            Captcha captcha,
            UserProfile profile
        ) {
            ItemId = itemId;
            SizeId = sizeId;
            StyleId = styleId;
            Quantity = quantity;
            Slug = slug;
            BasketResponse = basketResponse;
            CheckoutResponse = checkoutResponse;
            Pooky = pooky;
            PookyTicket = pookyTicket;
            Captcha = captcha;
            Profile = profile;
        }
    }
}
