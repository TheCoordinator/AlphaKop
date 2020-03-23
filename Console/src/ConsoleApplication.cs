using System.Threading.Tasks;
using AlphaKop.Core.Models.User;
using AlphaKop.Supreme.Flows;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop {
    public sealed class ConsoleApplication {
        private readonly ILogger<ConsoleApplication> logger;
        private readonly IPookyRepository pookyRepository;
        private readonly ISupremeRepository supremeRepository;
        
        public ConsoleApplication(
            IPookyRepository pookyRepository,
            ISupremeRepository supremeRepository,
            ILogger<ConsoleApplication> logger
        ) {
            this.pookyRepository = pookyRepository;
            this.supremeRepository = supremeRepository;
            this.logger = logger;
        }

        // Application starting point
        public void Run() {
            logger.LogDebug("Starting Application");
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
    }
}