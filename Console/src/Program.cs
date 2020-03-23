using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Core;
using AlphaKop.Core.Models.User;
using AlphaKop.Supreme.Flows;
using AlphaKop.Supreme.Repositories;
using AlphaKop.Supreme.Requests;

namespace AlphaKop {
    class Program {
        static ISupremeRepository supremeRepo = new SupremeRepository();
        static IPookyRepository pookyRepo = new PookyRepository();

        static async Task Main(string[] args) {
            var profile = CreateUserProfile();
            var job = CreateSupremeJob(profile: profile);

            var step = new FetchItemStep(supremeRepo);
            var flowArgument = new SupremeFlowArgument<Unit>(job: job, argument: Unit.Empty);

            await step.Execute(flowArgument);
        }

        private static SupremeJob CreateSupremeJob(UserProfile profile) {
            return new SupremeJob(
                profile: profile,
                categoryName: null,
                keywords: "fleece",
                style: "black",
                size: "xl"
            );
        }

        private static UserProfile CreateUserProfile() {
            return new UserProfile(
                name: "Name Lastname",
                email: "email@gmail.com",
                phoneNumber: "07777777777",
                new Address(
                    firstName: "Name",
                    lastName: "Lastname",
                    lineOne: "Line 1",
                    lineTwo: null,
                    lineThree: null,
                    city: "London",
                    state: null,
                    countryCode: "GB",
                    postCode: "SW1 2FX"
                ),
                cardDetails: new CardDetails(
                    cardNumber: "4242424242424242",
                    expiryMonth: "08",
                    expiryYear: "25",
                    cardVerification: "888"
                )
            );
        }

        static async Task RunDefaults() {
            try {
                var pooky = await pookyRepo.FetchPooky();
                Console.WriteLine($"Fetched Pooky Data {pooky.PageData.Mappings.Length}");

                var stock = await supremeRepo.FetchStock();
                Console.WriteLine($"Fetched Stock Items {stock.Items.Count}");

                // var itemDetails = await supremeRepo.FetchItemDetails(itemId: "304938");
                // Console.WriteLine($"Fetched Stock Items {itemDetails.Styles.Length}");

                var addBasketResponses = await supremeRepo.AddToBasket(
                    new AddBasketRequest(itemId: "304938",
                        sizeId: "63334",
                        styleId: "28700",
                        quantity: 1,
                        pooky: pooky)
                );
                Console.WriteLine($"Fetched Stock Items {addBasketResponses.Count()}");
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
