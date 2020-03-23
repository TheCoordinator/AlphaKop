using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Core;
using AlphaKop.Core.Flows;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchItemStep : ITaskStep<SupremeFlowArgument<Unit>> { }

    sealed class FetchItemStep : IFetchItemStep {
        private ISupremeRepository supremeRepository;

        public FetchItemStep(ISupremeRepository supremeRepository) {
            this.supremeRepository = supremeRepository;
        }

        public async Task Execute(SupremeFlowArgument<Unit> parameter) {
            try {
                var stockTask = await supremeRepository.FetchStock();
                var result = new SupremeFlowArgument<Stock>(
                    job: parameter.Job,
                    argument: stockTask
                );

                var item = FindItem(flow: result);
                var itemParameter = new SupremeFlowArgument<Item>(
                    job: parameter.Job,
                    argument: item
                );

                await new FetchItemDetailsStep(supremeRepository)
                    .Execute(itemParameter);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                
                await new FetchItemStep(supremeRepository)
                    .Execute(parameter);
            }
        }

        private Item FindItem(SupremeFlowArgument<Stock> flow) {
            var items = flow.Argument.Items;
            var selected = items
                .Select(
                    x => x.Value
                        .Where(x => x.Name.ToLower().Contains(flow.Job.Keywords.First()))
                );

            var item = selected.First()?.First();
            if (item == null) {
                throw new NullReferenceException("Could not find item");
            }

            return item.Value;
        }
    }
}
