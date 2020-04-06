namespace AlphaKop.Supreme.Models {
    public enum PookyRegion {
        EU,
        US
    }

    sealed class PookyRegionUtil {
        public static PookyRegion From(string region) {
            var uppercasedRegion = region.Trim().ToUpper();
            if (uppercasedRegion == "US") {
                return PookyRegion.US;
            } else {
                return PookyRegion.EU;
            }
        }
    }
}
