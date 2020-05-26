using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct Pooky {
        [JsonProperty("cookies")]
        public PookyCookies Cookies { get; }

        [JsonProperty("pageData")]
        public PookyPageData PageData { get; }

        [JsonConstructor]
        public Pooky(PookyCookies cookies, PookyPageData pageData) {
            Cookies = cookies;
            PageData = pageData;
        }
    }
}
