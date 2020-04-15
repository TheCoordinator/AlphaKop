using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Flows {
    public struct PookyTicketStepParameter {
        public SelectedItem SelectedItem { get; }
        public AddBasketResponse BasketResponse { get; }
        public string BasketTicket { get; }
        public Pooky Pooky { get; }

        public PookyTicketStepParameter(
            SelectedItem selectedItem,
            AddBasketResponse basketResponse,
            string basketTicket,
            Pooky pooky
        ) {
            SelectedItem = selectedItem;
            BasketResponse = basketResponse;
            BasketTicket = basketTicket;
            Pooky = pooky;
        }
    }
}