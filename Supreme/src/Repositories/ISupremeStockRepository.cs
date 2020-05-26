using AlphaKop.Supreme.Models;
using System.Threading.Tasks;

namespace AlphaKop.Supreme.Repositories {
    public interface ISupremeStockRepository {
        Task<Stock> FetchStock();
        Task<ItemDetails> FetchItemDetails(Item item);
    }
}
