using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Flows {
    public struct AddBasketStepParameter {
        public SelectedItem SelectedItem { get; }
        public Pooky Pooky { get; }

        public AddBasketStepParameter(
            SelectedItem selectedItem,
            Pooky pooky
        ) {
            SelectedItem = selectedItem;
            Pooky = pooky;
        }
    }
}