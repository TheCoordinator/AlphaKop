using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Flows {
    public struct SuccessStepInput : IStepInput {
        public SelectedItem SelectedItem { get; }
        public CheckoutResponse CheckoutResponse { get; }
        public SupremeJob Job { get; }

        public SuccessStepInput(
            SelectedItem selectedItem,
            CheckoutResponse checkoutResponse,
            SupremeJob job
        ) {
            SelectedItem = selectedItem;
            CheckoutResponse = checkoutResponse;
            Job = job;
        }
    }
}