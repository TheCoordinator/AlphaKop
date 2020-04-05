using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Repositories {
    public interface ISupremeRepository {
        Task<Stock> FetchStock();
        Task<ItemDetails> FetchItemDetails(Item item);
        Task<IEnumerable<ItemAddBasketSizeStock>> AddBasket(AddBasketRequest basketRequest);
    }
}
