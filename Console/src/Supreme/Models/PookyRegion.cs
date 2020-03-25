using AlphaKop.Supreme.Flows;

namespace AlphaKop.Supreme.Models {
    public enum PookyRegion {
        EU,
        US
    }

    sealed class PookyRegionUtil {
        public static PookyRegion From(SupremeRegion region) {
            switch (region) {
                case SupremeRegion.EU: return PookyRegion.EU;
                case SupremeRegion.US: return PookyRegion.US;
                default: return PookyRegion.EU;
            }
        }
    }
}
