using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Flows {
    public struct CaptchaStepInput : IStepInput {
        public SelectedItem SelectedItem { get; }
        public Pooky Pooky { get; }
        public CheckoutCookies CheckoutCookies { get; }
        public SupremeJob Job { get; }

        public CaptchaStepInput(
            SelectedItem selectedItem,
            Pooky pooky,
            CheckoutCookies checkoutCookies,
            SupremeJob job
        ) {
            SelectedItem = selectedItem;
            Pooky = pooky;
            CheckoutCookies = checkoutCookies;
            Job = job;
        }
    }
}