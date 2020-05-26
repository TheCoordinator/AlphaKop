using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Flows {
    public struct PookyTicketStepInput : IStepInput {
        public SelectedItem SelectedItem { get; }
        public AddBasketResponse BasketResponse { get; }
        public string BasketTicket { get; }
        public Pooky Pooky { get; }
        public SupremeJob Job { get; }

        public PookyTicketStepInput(
            SelectedItem selectedItem,
            AddBasketResponse basketResponse,
            string basketTicket,
            Pooky pooky,
            SupremeJob job
        ) {
            SelectedItem = selectedItem;
            BasketResponse = basketResponse;
            BasketTicket = basketTicket;
            Pooky = pooky;
            Job = job;
        }
    }
}