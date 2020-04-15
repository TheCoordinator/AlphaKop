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
        private readonly ILogger logger;

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

                var size = FindSize(item: parameter, style: style, job: job);

                logger.LogInformation(
                    JobEventId,
                    $"Fetched Size Item {parameter.Id}\n" +
                    size.ToString()
                );

                if (size.isStockAvailable == true) {
                    var pookyParam = new SelectedItemParameter(
                        item: parameter,
                        style: style,
                        size: size
                    );

                    await provider.CreateFetchPookyStep(job)
                        .Execute(pookyParam);
                } else {
                    logger.LogInformation(
                        JobEventId,
                        $"Stock Unavailable {parameter.Id} Retrying...\n" +
                        size.ToString()
                    );

                    await provider.CreateFetchItemDetailsStep(job, Retries + 1)
                        .Execute(parameter);
                }
            } catch (Exception ex) {
                logger.LogError(JobEventId, ex, "Failed to retrieve ItemDetails");

                await provider.CreateFetchItemDetailsStep(job, Retries + 1)
                    .Execute(parameter);
            }
        }

        #region Style

        private ItemStyle FindStyle(ItemDetails details, SupremeJob job) {
            var styles = details.Styles;

            if (styles.Count() == 0) {
                throw new StyleNotFoundException(null, itemId: details.Item.Id, styleName: job.Style);
            }

            ItemStyle? result;

            if (job.Style != null) {
                result = FindMatchingStyle(styles: styles, styleName: job.Style);
            } else {
                result = FindAvailableStyle(styles: styles);
            }

            if (result == null) {
                throw new StyleNotFoundException(null, itemId: details.Item.Id, styleName: job.Style);
            }

            return result.Value;
        }

        private ItemStyle? FindAvailableStyle(IEnumerable<ItemStyle> styles) {
            if (styles.Count() == 1) {
                return styles.FirstOrDefault();
            }

            return styles
                .FirstOrDefault(style => {
                    return style.Sizes
                        .Any(size => size.isStockAvailable == true);
                });
        }

        private ItemStyle? FindMatchingStyle(IEnumerable<ItemStyle> styles, string styleName) {
            var styleNames = styles
                .Select(style => style.Name);

            var result = textMatching.ExtractOne(styleName, choices: styleNames);

            if (result == null) {
                return null;
            }

            return styles.FirstOrDefault(style => style.Name == result.Value.Value);
        }

        #endregion

        #region Size

        private ItemSize FindSize(Item item, ItemStyle style, SupremeJob job) {
            var sizes = style.Sizes;

            if (sizes.Count() == 0) {
                throw new SizeNotFoundException(null, itemId: item.Id, sizeName: job.Size);
            }

            if (sizes.Count() == 1) {
                return sizes.FirstOrDefault();
            }

            ItemSize? result;

            if (job.Size != null) {
                result = FindMatchingSize(item: item, sizes: sizes, sizeName: job.Size);
            } else {
                result = FindAvailableSize(item: item, sizes: sizes);
            }

            if (result == null) {
                throw new SizeNotFoundException(null, itemId: item.Id, sizeName: job.Size);
            }

            return result.Value;
        }

        private ItemSize? FindAvailableSize(Item item, IEnumerable<ItemSize> sizes) {
            if (sizes.Count() == 1) {
                return sizes.FirstOrDefault();
            }

            return sizes
                .FirstOrDefault(size => size.isStockAvailable == true);
        }

        private ItemSize? FindMatchingSize(Item item, IEnumerable<ItemSize> sizes, string sizeName) {
            ItemSize? result;

            try {
                if (item.CategoryName?.ToLower() == "shoes") {
                    result = FindShoesSize(sizes: sizes, sizeName: sizeName);
                } else {
                    result = FindClothingSize(sizes: sizes, sizeName: sizeName);
                }
            } catch (Exception) {
                return FindSizeByText(sizes: sizes, sizeName: sizeName);
            }

            if (result != null) { return result; }

            return FindSizeByText(sizes: sizes, sizeName: sizeName);
        }

        private ItemSize? FindShoesSize(IEnumerable<ItemSize> sizes, string sizeName) {
            if (sizeName.Length == 1) {
                // For some reason textMathing can't detect number characters
                return sizes
                    .FirstOrDefault(size => size.Name.ToLower().Contains(sizeName.ToLower()));
            }

            return FindSizeByText(sizes: sizes, sizeName: sizeName);
        }

        private ItemSize? FindClothingSize(IEnumerable<ItemSize> sizes, string sizeName) {
            var clothingSize = ItemStyleSizeTypeUtil.From(sizeName);
            if (clothingSize == null) {
                return null;
            }

            return sizes
                .FirstOrDefault(size => ItemStyleSizeTypeUtil.From(size.Name) == clothingSize);
        }

        private ItemSize? FindSizeByText(IEnumerable<ItemSize> sizes, string sizeName) {
            var sizeNames = sizes
                .Select(size => size.Name);

            var result = textMatching.ExtractOne(sizeName, choices: sizeNames);

            return sizes.FirstOrDefault(size => size.Name == result?.Value);
        }

        #endregion
    }
}
