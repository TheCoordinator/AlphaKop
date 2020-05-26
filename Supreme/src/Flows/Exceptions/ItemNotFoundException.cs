using System;

namespace AlphaKop.Supreme.Flows {
    [Serializable]
    sealed class ItemNotFoundException : Exception {
        public string Keywords { get; }

        public ItemNotFoundException(string? message, string keywords) : base(message) {
            Keywords = keywords;
        }
    }
}
