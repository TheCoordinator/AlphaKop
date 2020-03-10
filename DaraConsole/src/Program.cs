using System;
using System.Threading.Tasks;
using DaraBot.Supreme.Services;

#nullable enable

namespace DaraBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ISupreme supreme = new DefaultSupreme();
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
