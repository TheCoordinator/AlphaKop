using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Flows {
    public struct SelectedItemParameter {
        public Item Item { get; }
        public ItemStyle Style { get; }
        public ItemSize Size { get; }

        public SelectedItemParameter(Item item, ItemStyle style, ItemSize size) {
            Item = item;
            Style = style;
            Size = size;
        }

        public override string ToString() {
            return $@"Item [{Item.Id}, {Item.Name}] Style [{Style.Id}, {Style.Name}] Size [{Size.Id}, {Size.Name}]]";
        }
    }
}