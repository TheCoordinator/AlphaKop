using AlphaKop.Core.Models.User;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutTotalsMobileRequest {
        public string SizeId { get; }
        public int Quantity { get; }
        public UserProfile Profile { get; }

        public CheckoutTotalsMobileRequest(
            string sizeId,
            int quantity,
            UserProfile profile
        ) {
            SizeId = sizeId;
            Quantity = quantity;
            Profile = profile;
        }
    }
}