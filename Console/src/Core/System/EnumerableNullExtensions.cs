using System.Collections.Generic;
using System.Linq;

namespace AlphaKop.Core.System.Extensions {
    public static class EnumerableNullExtensions {
        public static IEnumerable<T> FilterNull<T>(this IEnumerable<T> elements) {
            return elements
                .Where(e => e != null)
                .Select(e => e);
        }
    }
}