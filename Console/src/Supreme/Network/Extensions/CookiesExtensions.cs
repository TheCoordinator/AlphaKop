using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Network.Extensions {
    static class CookiesExtensions {
        public static string ToCookiesString(this IEnumerable<IEnumerable<Cookie>> cookies) {
            var cookiesStringArray = cookies
                .SelectMany(cookie => cookie)
                .ToHashSet(new CookieNameComparer())
                .ToList()
                .OrderBy(cookie => cookie.Name)
                .Select(cookie => cookie.ToString());

            return string.Join("; ", cookiesStringArray);
        }

        private sealed class CookieNameComparer : IEqualityComparer<Cookie> {
            public bool Equals([AllowNull] Cookie x, [AllowNull] Cookie y) {
                if (x == y) {
                    return true;
                }

                return x?.Name == y?.Name;
            }

            public int GetHashCode([DisallowNull] Cookie obj) {
                return obj.Name.GetHashCode();
            }
        }
    }
}
