using System;
using AlphaKop.Core.Models.User;
using AlphaKop.Supreme.Flows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AlphaKop {
    public sealed class ConsoleApplication {
        private readonly IServiceProvider provider;
        private readonly ILogger<ConsoleApplication> logger;

        public ConsoleApplication(
            IServiceProvider provider,
            ILogger<ConsoleApplication> logger
        ) {
            this.provider = provider;
            this.logger = logger;
        }

        public async void Run() {
            logger.LogDebug("Starting Application");
            
            var profile = CreateUserProfile();
            var job = CreateSupremeJob(profile: profile);

            var start = provider.GetRequiredService<ISupremeStartStep>();
            start.Job = job;
            
            await start.Execute(job);
        }

        private static SupremeJob CreateSupremeJob(UserProfile profile) {
            return new SupremeJob(
                jobId: "1",
                jobEventId: 1,
                profile: profile,
                region: SupremeRegion.EU,
                categoryName: null,
                keywords: "studded ls top",
                style: "navy",
                size: null
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