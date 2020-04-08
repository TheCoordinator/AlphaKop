using AlphaKop.Core.Captcha.Network;
using AlphaKop.Core.Models.User;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutRequest : ICheckoutRequest {
        public string ItemId { get; }
        public string SizeId { get; }
        public string StyleId { get; }
        public int Quantity { get; }
        public AddBasketResponse BasketResponse { get; }
        public Pooky Pooky { get; }
        public PookyTicket PookyTicket { get; }
        public Captcha Captcha { get; }
        public UserProfile Profile { get; }

        public CheckoutRequest(
            string itemId,
            string sizeId,
            string styleId,
            int quantity,
            AddBasketResponse basketResponse,
            Pooky pooky,
            PookyTicket pookyTicket,
            Captcha captcha,
            UserProfile profile
        ) {
            ItemId = itemId;
            SizeId = sizeId;
            StyleId = styleId;
            Quantity = quantity;
            BasketResponse = basketResponse;
            Pooky = pooky;
            PookyTicket = pookyTicket;
            Captcha = captcha;
            Profile = profile;
        }
    }
}
