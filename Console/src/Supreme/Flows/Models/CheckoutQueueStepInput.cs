using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Flows {
    public struct CheckoutQueueStepInput : IStepInput {
        public SelectedItem SelectedItem { get; }
        public ICheckoutRequest CheckoutRequest { get; }
        public CheckoutResponse CheckoutResponse { get; }
        public string Slug { get; }
        public SupremeJob Job { get; }

        public CheckoutQueueStepInput(
            SelectedItem selectedItem,
            ICheckoutRequest checkoutRequest,
            CheckoutResponse checkoutResponse,
            string slug,
            SupremeJob job
        ) {
            SelectedItem = selectedItem;
            CheckoutRequest = checkoutRequest;
            CheckoutResponse = checkoutResponse;
            Slug = slug;
            Job = job;
        }
    }
}