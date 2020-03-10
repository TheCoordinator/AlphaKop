using System.Threading.Tasks;
using DaraBot.Supreme.Entities;

namespace DaraBot.Supreme.Repositories
{
    public interface IPookyRepository
    {
        Task<Pooky> FetchPooky();
    }
}
