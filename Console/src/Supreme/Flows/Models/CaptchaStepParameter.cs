using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Flows {
    public struct CaptchaStepParameter {
        public SelectedItem SelectedItem { get; }
        public AddBasketResponse BasketResponse { get; }
        public Pooky Pooky { get; }
        public PookyTicket PookyTicket { get; }

        public CaptchaStepParameter(
            SelectedItem selectedItem,
            AddBasketResponse basketResponse,
            Pooky pooky,
            PookyTicket pookyTicket
        ) {
            SelectedItem = selectedItem;
            BasketResponse = basketResponse;
            Pooky = pooky;
            PookyTicket = pookyTicket;
        }
    }
}