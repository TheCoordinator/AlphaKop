using Newtonsoft.Json;

namespace AlphaKop.Supreme.Models {
    public struct PookyTicket {
        [JsonProperty("_ticket")]
        public string Ticket { get; }

        [JsonConstructor]
        public PookyTicket(string ticket) {
            Ticket = ticket;
        }
    }
}