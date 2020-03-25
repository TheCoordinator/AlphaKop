using System.Collections.Generic;
using System.Linq;

namespace AlphaKop.Supreme.Models {
    enum StyleSizeType {
        XXS,
        XS,
        S,
        M,
        L,
        XL,
        XXL
    }

    sealed class StyleSizeTypeUtil {
        public static StyleSizeType? From(string size) {
            if (IsXXSmall(size)) {
                return StyleSizeType.XXS;
            } else if (IsXSmall(size)) {
                return StyleSizeType.XS;
            } else if (IsSmall(size)) {
                return StyleSizeType.S;
            } else if (IsMedium(size)) {
                return StyleSizeType.M;
            } else if (IsLarge(size)) {
                return StyleSizeType.L;
            } else if (IsXLarge(size)) {
                return StyleSizeType.XL;
            } else if (IsXXLarge(size)) {
                return StyleSizeType.XXL;
            }

            return null;           
        }

        private static bool IsXXSmall(string size) {
            var sizes = new string[] { "xxs", "xxsmall", "extra extra small" };
            return IsSize(sizes, size);
        }

        private static bool IsXSmall(string size) {
            var sizes = new string[] { "xs", "xsmall", "extra small" };
            return IsSize(sizes, size);
        }

        private static bool IsSmall(string size) {
            var sizes = new string[] { "s", "small" };
            return IsSize(sizes, size);
        }

        private static bool IsMedium(string size) {
            var sizes = new string[] { "m", "medium" };
            return IsSize(sizes, size);
        }

        private static bool IsLarge(string size) {
            var sizes = new string[] { "l", "large" };
            return IsSize(sizes, size);
        }        

        private static bool IsXLarge(string size) {
            var sizes = new string[] { "xl", "xlarge", "extra large" };
            return IsSize(sizes, size);
        }        

        private static bool IsXXLarge(string size) {
            var sizes = new string[] { "xxl", "xxlarge", "extra extra large" };
            return IsSize(sizes, size);
        }        

        private static bool IsSize(IEnumerable<string> sizes, string size) {
            return sizes
                .Any(s => s.ToLower().Trim() == size.ToLower().Trim());
        }
    }
}