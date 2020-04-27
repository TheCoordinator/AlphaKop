namespace AlphaKop.Supreme.Network {
    public struct CheckoutTotalsMobileResponse {
        public string HtmlContent { get; }

        public CheckoutTotalsMobileResponse(string htmlContent) {
            HtmlContent = htmlContent;
        }
   }
}