using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Repositories {
    public interface ISupremeRepository {
        Task<Stock> FetchStock();
        Task<ItemDetails> FetchItemDetails(Item item);
        Task<AddBasketResponse> AddBasket(AddBasketRequest basketRequest);
        Task<CheckoutResponse> Checkout(CheckoutRequest request);
        Task<CheckoutResponse> CheckoutQueue(CheckoutQueueRequest request);
    }
}
