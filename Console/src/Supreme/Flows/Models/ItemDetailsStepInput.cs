using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Flows {
    public struct ItemDetailsStepInput: IStepInput {
        public Item Item { get; }
        public SupremeJob Job { get; }

        public ItemDetailsStepInput(Item item, SupremeJob job) {
            Item = item;
            Job = job;
        }
    }
}