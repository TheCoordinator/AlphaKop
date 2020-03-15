using System.Collections.Generic;
using DaraBot.Supreme.Models;

namespace DaraBot.Supreme.Requests.Extensions
{
    static class PookyStaticCookiesExtensions
    {
        public static Dictionary<string, string> ToCookies(this PookyStatic staticData)
        {
            return new Dictionary<string, string>() {
                { "pooky", staticData.Pooky },
                { "pooky_order_allow", staticData.PookyOrderAllow },
                { "pooky_use_cookie", staticData.PookyOrderAllow.ToString() },
            };
        }
    }
}