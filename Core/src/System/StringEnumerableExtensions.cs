using System.Collections.Generic;

namespace AlphaKop.Core.System.Extensions {
    public static class StringEnumerableExtensions {
        public static string JoinStrings(this IEnumerable<string?> strings, char separator) {
            var values = strings
                .FilterNull();
            
            return string.Join(separator, values);
        }
    }
}