using System.Threading.Tasks;
using DaraBot.Supreme.Models;

namespace DaraBot.Supreme.Repositories
{
    public interface IPookyRepository
    {
        Task<Pooky> FetchPooky();
    }
}
