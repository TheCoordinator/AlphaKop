using AlphaKop.Core.Models.User;

namespace AlphaKop.Supreme.Network {
    public struct Card3DSecureRequest {
        public string ItemId { get; }
        public string SizeId { get; }
        public string StyleId { get; }
        public int Quantity { get; }
        public UserProfile Profile { get; }

        public Card3DSecureRequest(
            string itemId,
            string sizeId,
            string styleId,
            int quantity,
            UserProfile profile
        ) {
            ItemId = itemId;
            SizeId = sizeId;
            StyleId = styleId;
            Quantity = quantity;
            Profile = profile;
        }
    }
}