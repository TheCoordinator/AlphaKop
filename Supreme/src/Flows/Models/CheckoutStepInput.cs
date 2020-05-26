using AlphaKop.Core.Captcha.Network;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Services;

namespace AlphaKop.Supreme.Flows {
    public struct CheckoutStepInput : IStepInput {
        public SelectedItem SelectedItem { get; }
        public Pooky Pooky { get; }
        public Captcha Captcha { get; }
        public Card3DSecureResponse? Card3DSecureResponse { get; }
        public CheckoutCookies Cookies { get; }
        public SupremeJob Job { get; }

        public CheckoutStepInput(
            SelectedItem selectedItem,
            Pooky pooky,
            Captcha captcha,
            Card3DSecureResponse? card3DSecureResponse,
            CheckoutCookies cookies,
            SupremeJob job
        ) {
            SelectedItem = selectedItem;
            Pooky = pooky;
            Captcha = captcha;
            Card3DSecureResponse = card3DSecureResponse;
            Cookies = cookies;
            Job = job;
        }
    }
}