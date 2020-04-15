using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Flows {
    public struct SuccessStepParameter {
        public SelectedItem SelectedItem { get; }
        public CheckoutResponse CheckoutResponse { get; }

        public SuccessStepParameter(
            SelectedItem selectedItem,
            CheckoutResponse checkoutResponse
        ) {
            SelectedItem = selectedItem;
            CheckoutResponse = checkoutResponse;
        }
    }
}