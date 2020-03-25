using System;

namespace AlphaKop.Supreme.Flows {
    [Serializable]
    sealed class StyleNotFoundException : Exception {
        public string ItemId { get; }
        public string? StyleName { get; }

        public StyleNotFoundException(string? message, string itemId, string? styleName) : base(message) {
            ItemId = ItemId;
            StyleName = styleName;
        }
    }
}
