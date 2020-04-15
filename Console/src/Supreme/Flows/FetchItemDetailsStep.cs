using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaKop.Core.Flows;
using AlphaKop.Core.Services.TextMatching;
using AlphaKop.Core.System.Extensions;
using AlphaKop.Supreme.Models;
using AlphaKop.Supreme.Repositories;
using Microsoft.Extensions.Logging;

namespace AlphaKop.Supreme.Flows {
    public interface IFetchItemDetailsStep : ITaskStep<ItemDetailsStepInput> { }

    public sealed class FetchItemDetailsStep : IFetchItemDetailsStep {
        private readonly ISupremeRepository supremeRepository;
        private readonly ITextMatching textMatching;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        public int Retries { get; set; }

        public FetchItemDetailsStep(
            ISupremeRepository supremeRepository,
            ITextMatching textMatching,
            IServiceProvider provider,
            ILogger<FetchItemDetailsStep> logger
        ) {
            this.supremeRepository = supremeRepository;
            this.textMatching = textMatching;
            this.provider = provider;
            this.logger = logger;
        }

        public async Task Execute(ItemDetailsStepInput input) {
            try {
                await Task.Delay(input.Job.StartDelay);

                var details = await supremeRepository.FetchItemDetails(item: input.Item);
                var style = FindStyle(details: details, job: input.Job);
                var size = FindSize(item: input.Item, style: style, job: input.Job);

                await PerformPostItemDetails(input: input, style: style, size: size);
            } catch (StyleNotFoundException ex) {
                logger.LogError(input.Job.ToEventId(), $"--FetchItemDetailsStep Style Not Found. Item [{ex.ItemId}] Style [{ex.StyleName}]");

                await RetryStep(input);
            } catch (SizeNotFoundException ex) {
                logger.LogError(input.Job.ToEventId(), $"--FetchItemDetailsStep Size Not Found. Item [{ex.ItemId}] Size [{ex.SizeName}]");

                await RetryStep(input);
            } catch (Exception ex) {
                logger.LogError(input.Job.ToEventId(), ex, "--FetchItemDetailsStep Unhandled Exception");

                await RetryStep(input);
            }
        }

        private async Task PerformPostItemDetails(ItemDetailsStepInput input, ItemStyle style, ItemSize size) {
            LogItemDetails(item: input.Item, style: style, size: size, job: input.Job);

            if (size.isStockAvailable == true) {
                await PerformPookyStep(input: input, style: style, size: size);
            } else {
                await RetryStep(input);
            }
        }

        private void LogItemDetails(Item item, ItemStyle style, ItemSize size, SupremeJob job) {
            var stockAvailable = size.isStockAvailable ? "Available" : "Unavailable";

            logger.LogInformation(
                job.ToEventId(),
                "--FetchItemDetailsStep Style and Size Fetched. " +
                $"Stock [{stockAvailable}] " +
                $"Item [{item.Id}, {item.Name}] " +
                $"Style [{style.Id}, {style.Name}] " +
                $"Size [{size.Id}, {size.Name}] " +
                $"Job Style [{job.Style}] " +
                $"Job Size [{job.Size}]"
            );
        }

        private async Task PerformPookyStep(ItemDetailsStepInput input, ItemStyle style, ItemSize size) {
            var selectedItem = new SelectedItem(
                item: input.Item,
                style: style,
                size: size
            );

            var pookyInput = new PookyStepInput(
                selectedItem: selectedItem,
                job: input.Job
            );

            await provider.CreateStep<PookyStepInput, IFetchPookyStep>()
                .Execute(pookyInput);
        }

        private async Task RetryStep(ItemDetailsStepInput input) {
            await provider.CreateStep<ItemDetailsStepInput, IFetchItemDetailsStep>(Retries + 1)
                .Execute(input);
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
                return styles.FirstOrNull();
            }

            return styles
                .FirstOrNull(style => {
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

            return styles.FirstOrNull(style => style.Name == result.Value.Value);
        }

        #endregion

        #region Size

        private ItemSize FindSize(Item item, ItemStyle style, SupremeJob job) {
            var sizes = style.Sizes;

            if (sizes.Count() == 0) {
                throw new SizeNotFoundException(null, itemId: item.Id, sizeName: job.Size);
            }

            if (sizes.Count() == 1) {
                return sizes.First();
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
                return sizes.FirstOrNull();
            }

            return sizes
                .FirstOrNull(size => size.isStockAvailable == true);
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
                    .FirstOrNull(size => size.Name.ToLower().Contains(sizeName.ToLower()));
            }

            return FindSizeByText(sizes: sizes, sizeName: sizeName);
        }

        private ItemSize? FindClothingSize(IEnumerable<ItemSize> sizes, string sizeName) {
            var clothingSize = ItemStyleSizeTypeUtil.From(sizeName);
            if (clothingSize == null) {
                return null;
            }

            return sizes
                .FirstOrNull(size => ItemStyleSizeTypeUtil.From(size.Name) == clothingSize);
        }

        private ItemSize? FindSizeByText(IEnumerable<ItemSize> sizes, string sizeName) {
            var sizeNames = sizes
                .Select(size => size.Name);

            var result = textMatching.ExtractOne(sizeName, choices: sizeNames);

            return sizes.FirstOrNull(size => size.Name == result?.Value);
        }

        #endregion
    }
}
