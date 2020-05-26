namespace AlphaKop.Supreme.Flows {
    public struct CheckoutQueueStepInput : IStepInput {
        public SelectedItem SelectedItem { get; }
        public CheckoutCookies CheckoutCookies { get; }
        public string Slug { get; }
        public SupremeJob Job { get; }

        public CheckoutQueueStepInput(
            SelectedItem selectedItem,
            CheckoutCookies checkoutCookies,
            string slug,
            SupremeJob job
        ) {
            SelectedItem = selectedItem;
            CheckoutCookies = checkoutCookies;
            Slug = slug;
            Job = job;
        }
    }
}