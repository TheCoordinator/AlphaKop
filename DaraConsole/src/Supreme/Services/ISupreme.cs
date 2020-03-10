using System.Threading.Tasks;
using DaraBot.Supreme.Entities;

namespace DaraBot.Supreme.Services
{
    interface ISupreme
    {
        Task<Stock> FetchStock();
    }
}
