namespace AlphaKop.Supreme.Flows {
    public struct InitialStepInput : IStepInput {
        public SupremeJob Job { get; }

        public InitialStepInput(SupremeJob job) {
            Job = job;
        }
    }
}