using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Flows {
    public struct CaptchaStepParameter {
        public Item Item { get; }
        public ItemStyle Style { get; }
        public ItemSize Size { get; }
        public Pooky Pooky { get; }

        public CaptchaStepParameter(
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