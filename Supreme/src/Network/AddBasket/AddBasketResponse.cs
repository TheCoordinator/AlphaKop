using System.Collections.Generic;

namespace AlphaKop.Supreme.Network {
    public struct AddBasketResponse {
        public IEnumerable<ItemAddBasketSizeStock> ItemSizesStock { get; }
        public string? Ticket { get; }

        public AddBasketResponse(
            IEnumerable<ItemAddBasketSizeStock> itemSizesStock,
            string? ticket
        ) {
            ItemSizesStock = itemSizesStock;
            Ticket = ticket;
        }
    }
}