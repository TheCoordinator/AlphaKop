using AlphaKop.Core.Captcha.Network;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Flows {
    public struct CheckoutStepInput : IStepInput {
        public SelectedItem SelectedItem { get; }
        public AddBasketResponse BasketResponse { get; }
        public Pooky Pooky { get; }
        public PookyTicket PookyTicket { get; }
        public Captcha Captcha { get; }
        public SupremeJob Job { get; }

        public CheckoutStepInput(
            SelectedItem selectedItem,
            AddBasketResponse basketResponse,
            Pooky pooky,
            PookyTicket pookyTicket,
            Captcha captcha,
            SupremeJob job
        ) {
            SelectedItem = selectedItem;
            BasketResponse = basketResponse;
            Pooky = pooky;
            PookyTicket = pookyTicket;
            Captcha = captcha;
            Job = job;
        }
    }
}