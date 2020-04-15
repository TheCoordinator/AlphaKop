using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaKop.Core.System.Extensions {
    public static class EnumerableNullExtensions {
        public static T? FirstOrNull<T>(this IEnumerable<T> values) where T : struct {
            try {
                return values.First();
            } catch (InvalidOperationException) {
                return null;
            }
        }

        public static T? FirstOrNull<T>(this IEnumerable<T> values, Func<T, bool> predicate) where T : struct {
            try {
                return values.First(predicate);
            } catch (InvalidOperationException) {
                return null;
            }
        }

        public static IEnumerable<T> FilterNull<T>(this IEnumerable<T> elements) {
            return elements
                .Where(e => e != null)
                .Select(e => e);
        }
    }
}