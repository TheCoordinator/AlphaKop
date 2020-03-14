namespace DaraBot.Supreme.Requests
{
    public struct AddBasketRequest
    {
        public string itemId { get; internal set; }
        public string sizeId { get; internal set; }
        public int styleId { get; internal set; }
        public int quantity { get; internal set; }

        public AddBasketRequest(string itemId,
                                string sizeId,
                                int styleId,
                                int quantity)
        {
            this.itemId = itemId;
            this.sizeId = sizeId;
            this.styleId = styleId;
            this.quantity = quantity;
        }
    }
}