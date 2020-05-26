using System.Collections.Generic;
using AlphaKop.Supreme.Models;
using Newtonsoft.Json;

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
