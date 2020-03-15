using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Supreme.Repositories;
using AlphaKop.Supreme.Requests;

namespace AlphaKop {
    class Program {
        static async Task Main (string[] args) {
            ISupremeRepository supremeRepo = new SupremeRepository ();
            IPookyRepository pookyRepo = new PookyRepository ();

            try {
                var pooky = await pookyRepo.FetchPooky ();
                Console.WriteLine ($"Fetched Pooky Data {pooky.PageData.Mappings.Length}");

                var stock = await supremeRepo.FetchStock ();
                Console.WriteLine ($"Fetched Stock Items {stock.Items.Count}");

                var itemDetails = await supremeRepo.FetchItemDetails (itemId: "304938");
                Console.WriteLine ($"Fetched Stock Items {itemDetails.Styles.Length}");

                var addBasketResponses = await supremeRepo.AddToBasket (
                    new AddBasketRequest (itemId: "304938",
                        sizeId: "63334",
                        styleId: "28700",
                        quantity : 1,
                        pooky : pooky)
                );
                Console.WriteLine ($"Fetched Stock Items {addBasketResponses.Count()}");
            }
            catch (Exception ex) {
                Console.WriteLine (ex.Message);
            }

            Console.ReadLine ();
        }
    }
}
