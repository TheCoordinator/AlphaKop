using DaraBot.Supreme.Models;

namespace DaraBot.Supreme.Requests
{
    public struct AddBasketRequest
    {
        public string ItemId { get; internal set; }
        public string SizeId { get; internal set; }
        public string StyleId { get; internal set; }
        public int Quantity { get; internal set; }
        public PookyAddToCart AddToCart { get; internal set; }

        public AddBasketRequest(string itemId,
                                string sizeId,
                                string styleId,
                                int quantity,
                                PookyAddToCart addToCart)
        {
            ItemId = itemId;
            SizeId = sizeId;
            StyleId = styleId;
            Quantity = quantity;
            AddToCart = addToCart;
        }
    }
}