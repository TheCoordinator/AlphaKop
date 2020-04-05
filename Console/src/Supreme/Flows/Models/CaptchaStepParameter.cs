using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Flows {
    public struct CaptchaStepParameter {
        public SelectedItemParameter SelectedItem { get; }
        public Pooky Pooky { get; }

        public CaptchaStepParameter(
            SelectedItemParameter selectedItem,
            Pooky pooky
        ) {
            SelectedItem = selectedItem;
            Pooky = pooky;
        }
    }
}