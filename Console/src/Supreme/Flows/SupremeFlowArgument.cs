namespace AlphaKop.Supreme.Flows {
    public struct SupremeFlowArgument<TArgument> {
        public SupremeJob Job { get; internal set; }
        public TArgument Argument { get; internal set; }

        public SupremeFlowArgument(SupremeJob job, TArgument argument) {
            Job = job;
            Argument = argument;
        }
    }
}