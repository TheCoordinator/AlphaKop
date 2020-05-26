using System.Threading.Tasks;
using AlphaKop.Supreme.Models;

namespace AlphaKop.Supreme.Repositories {
    public interface ISupremeStockRepository {
        Task<Stock> FetchStock();
        Task<ItemDetails> FetchItemDetails(Item item);
    }
}
