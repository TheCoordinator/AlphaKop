using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Flows {
    public struct AddBasketStepInput : IStepInput {
        public SelectedItem SelectedItem { get; }
        public Pooky Pooky { get; }
        public SupremeJob Job { get; }

        public AddBasketStepInput(
            SelectedItem selectedItem,
            Pooky pooky,
            SupremeJob job
        ) {
            SelectedItem = selectedItem;
            Pooky = pooky;
            Job = job;
        }
    }
}