using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Requests;
using AlphaKop.Supreme.Responses;

namespace AlphaKop.Supreme.Repositories {
    public interface ISupremeRepository {
        Task<Stock> FetchStock();
        Task<ItemDetails> FetchItemDetails(Item item);
        Task<IEnumerable<AddBasketResponse>> AddBasket(AddBasketRequest basketRequest);
    }
}
