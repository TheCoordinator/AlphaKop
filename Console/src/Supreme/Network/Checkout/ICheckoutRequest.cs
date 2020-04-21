using System.Collections.Generic;
using System.Net;
using AlphaKop.Core.Captcha.Network;
using AlphaKop.Core.Models.User;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Network {
    public interface ICheckoutRequest {
        string ItemId { get; }
        string SizeId { get; }
        string StyleId { get; }
        int Quantity { get; }
        IEnumerable<Cookie> Cookies { get; }
        Pooky Pooky { get; }
        Captcha Captcha { get; }
        UserProfile Profile { get; }
    }
}
