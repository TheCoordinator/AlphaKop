using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Core.Services.TextMatching;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchItemDetailsStep : ITaskStep<Item, SupremeJob> { }

    sealed class FetchItemDetailsStep : BaseStep<Item>, IFetchItemDetailsStep {
        private readonly ISupremeRepository supremeRepository;
        private readonly ITextMatching textMatching;
        private readonly ILogger<FetchItemDetailsStep> logger;

        public FetchItemDetailsStep(
            ISupremeRepository supremeRepository,
            ITextMatching textMatching,
            IServiceProvider provider,
            ILogger<FetchItemDetailsStep> logger
        ) : base(provider) {
            this.supremeRepository = supremeRepository;
            this.textMatching = textMatching;
            this.logger = logger;
        }

        protected override async Task Execute(Item parameter, SupremeJob job) {
            try {
                var details = await supremeRepository.FetchItemDetails(item: parameter);

                logger.LogInformation(
                    JobEventId,
                    $"Fetched Details Item {parameter.Id}\n" +
                    details.ToString()
                );

                var style = FindStyle(details: details, job: job);

                logger.LogInformation(
                    JobEventId,
                    $"Fetched Style Item {parameter.Id}\n" +
                    style.ToString()
                );

                // var size = FindSize(style: style, job: job);

                // logger.LogInformation(
                //     JobEventId,
                //     $"Fetched Size Item {parameter.Id}\n" +
                //     size.ToString()
                // );


            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "Failed to retrieve ItemDetails");

                await provider.CreateFetchItemDetailsStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }

        #region Style
        private Style FindStyle(ItemDetails details, SupremeJob job) {
            var styles = details.Styles;

            if (styles.Count() == 0) {
                throw new StyleNotFoundException(null, itemId: details.Item.Id, styleName: job.Style);
            }

            Style? result;

            if (job.Style != null) {
                result = FindMatchingStyle(styles: styles, styleName: job.Style);
            } else {
                result = FindAvailableStyle(styles: styles, job: job);
            }

            if (result == null) {
                throw new StyleNotFoundException(null, itemId: details.Item.Id, styleName: job.Style);
            }

            return result.Value;
        }

        private Style? FindAvailableStyle(IEnumerable<Style> styles, SupremeJob job) {
            if (styles.Count() == 1) {
                return styles.First();
            }

            return styles
                .First(style => {
                    return style.Sizes
                        .Any(size => size.isStockAvailable == true);
                });
        }

        private Style? FindMatchingStyle(IEnumerable<Style> styles, string styleName) {
            var styleNames = styles
                .Select(style => style.Name);

            var result = textMatching.ExtractOne(styleName, choices: styleNames);

            if (result == null) {
                return null;
            }

            return styles.First(style => style.Name == result.Value.Value);
        }

        #endregion
    
        #region Size
            
        #endregion
    }
}
