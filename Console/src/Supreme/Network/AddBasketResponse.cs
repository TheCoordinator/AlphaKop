using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AlphaKop.Supreme.Network {
    public struct AddBasketResponse {
        public IEnumerable<ItemAddBasketSizeStock> ItemSizesStock { get; }
        public IEnumerable<Cookie> ResponseCookies { get; }

        public string? Ticket {
            get {
                return ResponseCookies
                    .FirstOrDefault(cookie => cookie.Name == "ticket")?
                    .Value;
            }
        }

        public AddBasketResponse(
            IEnumerable<ItemAddBasketSizeStock> itemSizesStock,
            IEnumerable<Cookie> responseCookies
        ) {
            ItemSizesStock = itemSizesStock;
            ResponseCookies = responseCookies;
        }
    }
}