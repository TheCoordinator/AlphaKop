using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Network;
using System.Threading.Tasks;

namespace AlphaKop.Supreme.Repositories {
    public interface IPookyRepository {
        Task<Pooky> FetchPooky(PookyRegion region);
        Task<Pooky> FetchPooky(PookyItemRequest request);
        Task<PookyTicket> FetchPookyTicket(PookyRegion region, string ticket);
    }
}
