using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Network {
    public struct PookyItemRequest {
        public PookyRegion Region { get; }
        public string StyleId { get; }
        public string SizeId { get; }

        public PookyItemRequest(PookyRegion region, string styleId, string sizeId) {
            Region = region;
            StyleId = styleId;
            SizeId = sizeId;
        }
    }
}
