using AlphaKop.Core.Models.User;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Network {
    public interface ICheckoutRequest {
        string ItemId { get; }
        string SizeId { get; }
        string StyleId { get; }
        int Quantity { get; }
        AddBasketResponse BasketResponse { get; }
        Pooky Pooky { get; }
        PookyTicket PookyTicket { get; }
        UserProfile Profile { get; }
    }
}
