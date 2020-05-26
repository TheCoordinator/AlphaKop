namespace AlphaKop.Supreme.Network {
    public struct CheckoutResponse {
        public CheckoutStatusResponse StatusResponse { get; }
        public string? Ticket { get; }

        public CheckoutResponse(CheckoutStatusResponse statusResponse, string? ticket) {
            StatusResponse = statusResponse;
            Ticket = ticket;
        }
    }
}
