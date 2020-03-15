using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Requests {
    public struct AddBasketRequest {
        public string ItemId { get; internal set; }
        public string SizeId { get; internal set; }
        public string StyleId { get; internal set; }
        public int Quantity { get; internal set; }
        public Pooky Pooky { get; internal set; }

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
