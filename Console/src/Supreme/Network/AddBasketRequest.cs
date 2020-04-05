using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Network {
    public struct AddBasketRequest {
        public string ItemId { get; }
        public string SizeId { get; }
        public string StyleId { get; }
        public int Quantity { get; }
        public Pooky Pooky { get; }

        public AddBasketRequest(
            string itemId,
            string sizeId,
            string styleId,
            int quantity,
            Pooky pooky
        ) {
            ItemId = itemId;
            SizeId = sizeId;
            StyleId = styleId;
            Quantity = quantity;
            Pooky = pooky;
        }
    }
}
