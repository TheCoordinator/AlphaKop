using System;
using System.Threading.Tasks;
using DaraBot.Supreme.Repositories;

namespace DaraBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ISupremeRepository supreme = new SupremeRepository();
            
            try
            {
                var result = await supreme.FetchStock();
                Console.WriteLine($"Fetched Items {result.Items.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
