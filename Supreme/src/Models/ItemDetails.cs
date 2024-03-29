using System.Collections.Generic;

namespace AlphaKop.Supreme.Models {
    public struct ItemDetails {
        public Item Item { get; }
        public IEnumerable<ItemStyle> Styles { get; }

        public ItemDetails(Item item, IEnumerable<ItemStyle> styles) {
            Item = item;
            Styles = styles;
        }

        public override bool Equals(object? obj) {
            if (obj == null) { return false; }
            return ((ItemDetails)obj).Item.Id == Item.Id;
        }

        public override int GetHashCode() {
            return Item.Id.GetHashCode();
        }
    }
}
