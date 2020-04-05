using System.Collections.Generic;
using System.Net;

namespace AlphaKop.Supreme.Network {
    public struct AddBasketResponse {
        public IEnumerable<ItemAddBasketSizeStock> ItemSizesStock { get; }
        public IEnumerable<Cookie> ResponseCookies { get; }

        public AddBasketResponse(
            IEnumerable<ItemAddBasketSizeStock> itemSizesStock,
            IEnumerable<Cookie> responseCookies
        ) {
            ItemSizesStock = itemSizesStock;
            ResponseCookies = responseCookies;
        }
    }
}