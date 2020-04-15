using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Flows {
    public struct CaptchaStepInput: IStepInput {
        public SelectedItem SelectedItem { get; }
        public AddBasketResponse BasketResponse { get; }
        public Pooky Pooky { get; }
        public PookyTicket PookyTicket { get; }
        public SupremeJob Job { get; }

        public CaptchaStepInput(
            SelectedItem selectedItem,
            AddBasketResponse basketResponse,
            Pooky pooky,
            PookyTicket pookyTicket,
            SupremeJob job
        ) {
            SelectedItem = selectedItem;
            BasketResponse = basketResponse;
            Pooky = pooky;
            PookyTicket = pookyTicket;
            Job = job;
        }
    }
}