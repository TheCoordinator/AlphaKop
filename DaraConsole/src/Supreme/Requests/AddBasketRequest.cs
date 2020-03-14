namespace DaraBot.Supreme.Requests
{
    public struct AddBasketRequest
    {
        public long itemId { get; internal set; }
        public long sizeId { get; internal set; }
        public int styleId { get; internal set; }
        public int quantity { get; internal set; }

        public AddBasketRequest(long itemId,
                                long sizeId,
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