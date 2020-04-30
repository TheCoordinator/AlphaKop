using System.Threading.Tasks;
using AlphaKop.Supreme.Network;

namespace AlphaKop.Supreme.Repositories {
    public interface ISupremeCheckoutRepository {
        Task<AddBasketResponse> AddBasket(AddBasketRequest basketRequest);
        Task<CheckoutTotalsMobileResponse> FetchCheckoutTotalsMobile(CheckoutTotalsMobileRequest request);
        Task<CheckoutResponse> Checkout(CheckoutRequest request);
        Task<CheckoutResponse> CheckoutQueue(CheckoutQueueRequest request);
    }
}
