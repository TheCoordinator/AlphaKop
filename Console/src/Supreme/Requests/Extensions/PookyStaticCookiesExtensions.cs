using System.Collections.Generic;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Requests.Extensions
{
    static class PookyStaticCookiesExtensions
    {
        public static Dictionary<string, string> ToCookies(this PookyStatic staticData)
        {
            return new Dictionary<string, string>() {
                { "pooky_use_cookie", staticData.PookyUseCookie.ToString() },
                { "pooky_order_allow", staticData.PookyOrderAllow },
                { "pooky", staticData.Pooky },
                { "_supreme_sess", staticData.SupremeSession }
            };
        }
    }
}