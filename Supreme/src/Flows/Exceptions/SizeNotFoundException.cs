using System;

namespace AlphaKop.Supreme.Flows {
    [Serializable]
    sealed class SizeNotFoundException : Exception {
        public string ItemId { get; }
        public string? SizeName { get; }

        public SizeNotFoundException(string? message, string itemId, string? sizeName) : base(message) {
            ItemId = ItemId;
            SizeName = sizeName;
        }
    }
}
