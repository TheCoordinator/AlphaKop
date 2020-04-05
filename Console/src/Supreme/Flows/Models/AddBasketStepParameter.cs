using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Flows {
    public struct AddBasketStepParameter {
        public Item Item { get; }
        public ItemStyle Style { get; }
        public ItemSize Size { get; }
        public Pooky Pooky { get; }

        public AddBasketStepParameter(
            Item item,
            ItemStyle style,
            ItemSize size,
            Pooky pooky
        ) {
            Item = item;
            Style = style;
            Size = size;
            Pooky = pooky;
        }
    }
}