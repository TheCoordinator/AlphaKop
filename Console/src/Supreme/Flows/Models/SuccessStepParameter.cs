using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Flows {
    public struct SuccessStepParameter {
        public SelectedItemParameter SelectedItem { get; }
        public CheckoutResponse CheckoutResponse { get; }

        public SuccessStepParameter(
            SelectedItemParameter selectedItem,
            CheckoutResponse checkoutResponse
        ) {
            SelectedItem = selectedItem;
            CheckoutResponse = checkoutResponse;
        }
    }
}