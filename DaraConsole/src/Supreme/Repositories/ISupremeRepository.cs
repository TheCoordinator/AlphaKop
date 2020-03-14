using System.Threading.Tasks;
using DaraBot.Supreme.Models;

namespace DaraBot.Supreme.Repositories
{
    public interface ISupremeRepository
    {
        Task<Stock> FetchStock();
        Task<ItemDetails> FetchItemDetails(string itemId);
    }
}
