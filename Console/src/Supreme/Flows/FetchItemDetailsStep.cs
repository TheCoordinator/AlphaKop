using System;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchItemDetailsStep : ITaskStep<SupremeFlowArgument<Item>> { }

    sealed class FetchItemDetailsStep : IFetchItemDetailsStep {
        private ISupremeRepository supremeRepository;

        public FetchItemDetailsStep(ISupremeRepository supremeRepository) {
            this.supremeRepository = supremeRepository;
        }

        public async Task Execute(SupremeFlowArgument<Item> parameter) {
            try {
                var itemDetails = await supremeRepository.FetchItemDetails(item: parameter.Argument);
                var result = new SupremeFlowArgument<ItemDetails>(
                    job: parameter.Job,
                    argument: itemDetails
                );

                Console.WriteLine(result.Argument);

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);

                await new FetchItemDetailsStep(supremeRepository)
                    .Execute(parameter);
            }
        }


    }
}
