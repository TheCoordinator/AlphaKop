using System.Collections.Generic;
using System.Linq;
using System.Net;

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