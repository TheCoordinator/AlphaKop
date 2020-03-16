using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Requests;
using AlphaKop.Supreme.Responses;

namespace AlphaKop.Supreme.Repositories {
    public interface ISupremeRepository {
        Task<Stock> FetchStock();
        Task<ItemDetails> FetchItemDetails(string itemId);
        Task<IEnumerable<AddBasketResponse>> AddToBasket(AddBasketRequest basketRequest);
    }
}
