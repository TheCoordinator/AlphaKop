using System.Threading.Tasks;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Repositories {
    public interface IPookyRepository {
        Task<Pooky> FetchPooky();
    }
}