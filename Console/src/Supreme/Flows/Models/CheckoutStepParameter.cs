using AlphaKop.Core.Captcha.Network;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Flows {
    public struct CheckoutStepParameter {
        public SelectedItemParameter SelectedItem { get; }
        public AddBasketResponse BasketResponse { get; }
        public Pooky Pooky { get; }
        public PookyTicket PookyTicket { get; }
        public Captcha Captcha { get; }

        public CheckoutStepParameter(
            SelectedItemParameter selectedItem,
            AddBasketResponse basketResponse,
            Pooky pooky,
            PookyTicket pookyTicket,
            Captcha captcha
        ) {
            SelectedItem = selectedItem;
            BasketResponse = basketResponse;
            Pooky = pooky;
            PookyTicket = pookyTicket;
            Captcha = captcha;
        }
    }
}