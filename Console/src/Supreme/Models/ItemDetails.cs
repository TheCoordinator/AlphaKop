using System.Linq;

namespace AlphaKop.Supreme.Models {
    public struct ItemDetails {
        public Item Item { get; internal set; }
        public Style[] Styles { get; internal set; }

        public ItemDetails(Item item, Style[] styles) {
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

        public override string ToString() {
            var styles = string.Join("\n\n", Styles.Select(s => s.ToString()));

            return
                $"Id: {Item.Id}\n" +
                $"Name: {Item.Name}\n" +
                $"Styles: \n{styles}";
        }
    }
}
