using AlphaKop.Core.Captcha.Network;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Flows {
    public struct CheckoutStepInput : IStepInput {
        public SelectedItem SelectedItem { get; }
        public Pooky Pooky { get; }
        public Captcha Captcha { get; }
        public CheckoutCookies Cookies { get; }
        public SupremeJob Job { get; }

        public CheckoutStepInput(
            SelectedItem selectedItem,
            Pooky pooky,
            Captcha captcha,
            CheckoutCookies cookies,
            SupremeJob job
        ) {
            SelectedItem = selectedItem;
            Pooky = pooky;
            Captcha = captcha;
            Cookies = cookies;
            Job = job;
        }
    }
}