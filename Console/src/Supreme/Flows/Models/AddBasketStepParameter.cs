using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Flows {
    public struct AddBasketStepParameter {
        public SelectedItemParameter SelectedItem { get; }
        public Pooky Pooky { get; }

        public AddBasketStepParameter(
            SelectedItemParameter selectedItem,
            Pooky pooky
        ) {
            SelectedItem = selectedItem;
            Pooky = pooky;
        }
    }
}