namespace AlphaKop.Supreme.Network {
    public struct CheckoutTotalsMobileResponse {
        public string HtmlContent { get; }
        public string? Ticket { get; }

        public CheckoutTotalsMobileResponse(string htmlContent, string? ticket) {
            HtmlContent = htmlContent;
            Ticket = ticket;
        }
    }
}