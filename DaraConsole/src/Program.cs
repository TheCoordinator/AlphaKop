using System;
using System.Threading.Tasks;
using DaraBot.Supreme.Repositories;

namespace DaraBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ISupremeRepository supremeRepo = new SupremeRepository();
            IPookyRepository pookyRepo = new PookyRepository();
            
            try
            {
                var pooky = await pookyRepo.FetchPooky();
                Console.WriteLine($"Fetched Pooky Data {pooky.PageData.Mappings.Length}");

                var stock = await supremeRepo.FetchStock();
                Console.WriteLine($"Fetched Stock Items {stock.Items.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
