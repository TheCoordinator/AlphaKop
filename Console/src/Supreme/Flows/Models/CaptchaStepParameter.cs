using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Flows {
    public struct CaptchaStepParameter {
        public Item Item { get; internal set; }
        public Style Style { get; internal set; }
        public Size Size { get; internal set; }
        public Pooky Pooky { get; internal set; }

        public CaptchaStepParameter(
            Item item,
            Style style,
            Size size,
            Pooky pooky
        ) {
            Item = item;
            Style = style;
            Size = size;
            Pooky = pooky;
        }
    }
}