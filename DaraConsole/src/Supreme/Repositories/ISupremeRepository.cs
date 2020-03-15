using System.Collections.Generic;
using System.Threading.Tasks;
using DaraBot.Supreme.Models;
using DaraBot.Supreme.Requests;
using DaraBot.Supreme.Responses;

namespace DaraBot.Supreme.Repositories {
    public interface ISupremeRepository {
        Task<Stock> FetchStock();
        Task<ItemDetails> FetchItemDetails(string itemId);
        Task<IEnumerable<AddBasketResponse>> AddToBasket(AddBasketRequest basketRequest);
    }
}
