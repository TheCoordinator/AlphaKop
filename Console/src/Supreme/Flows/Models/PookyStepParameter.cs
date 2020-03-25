using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Flows {
    public struct PookyStepParameter {
        public Item Item { get; internal set; }
        public Style Style { get; internal set; }
        public Size Size { get; internal set; }

        public PookyStepParameter(Item item, Style style, Size size) {
            Item = item;
            Style = style;
            Size = size;
        }
    }
}