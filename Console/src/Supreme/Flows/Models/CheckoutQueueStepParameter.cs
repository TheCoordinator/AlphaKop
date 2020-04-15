using AlphaKop.Core.Captcha.Network;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Flows {
    public struct CheckoutQueueStepParameter {
        public SelectedItem SelectedItem { get; }
        public ICheckoutRequest CheckoutRequest { get; }
        public CheckoutResponse CheckoutResponse { get; }

        public CheckoutQueueStepParameter(
            SelectedItem selectedItem,
            ICheckoutRequest checkoutRequest,
            CheckoutResponse checkoutResponse
        ) {
            SelectedItem = selectedItem;
            CheckoutRequest = checkoutRequest;
            CheckoutResponse = checkoutResponse;
        }
    }
}