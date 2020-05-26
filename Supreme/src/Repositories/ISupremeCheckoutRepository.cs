using AlphaKop.Supreme.Network;
using System.Threading.Tasks;

namespace AlphaKop.Supreme.Repositories {
    public interface ISupremeCheckoutRepository {
        Task<AddBasketResponse> AddBasket(AddBasketRequest basketRequest);
        Task<CheckoutTotalsMobileResponse> FetchCheckoutTotalsMobile(CheckoutTotalsMobileRequest request);
        Task<CheckoutResponse> Checkout(CheckoutRequest request);
        Task<CheckoutResponse> CheckoutQueue(CheckoutQueueRequest request);
    }
}
