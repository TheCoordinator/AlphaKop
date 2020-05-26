namespace AlphaKop.Supreme.Flows {
    public struct PookyStepInput : IStepInput {
        public SelectedItem SelectedItem { get; }
        public SupremeJob Job { get; }

        public PookyStepInput(SelectedItem selectedItem, SupremeJob job) {
            SelectedItem = selectedItem;
            Job = job;
        }
    }
}