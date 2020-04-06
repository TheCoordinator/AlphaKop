namespace AlphaKop.Core.System.Extensions {
    public static class StringNullExtensions {
        public static string? NullIfEmptyTrimmed(this string? stringValue) {
            return stringValue?.Trim()?.NullIfEmpty();
        }

        public static string? NullIfEmpty(this string? stringValue) {
            return stringValue?.Length == 0 ? null : stringValue;
        }
    }
}