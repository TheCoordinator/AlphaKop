using FuzzySharp;
using FuzzySharp.SimilarityRatio;
using FuzzySharp.SimilarityRatio.Scorer;
using FuzzySharp.SimilarityRatio.Scorer.Composite;
using System.Collections.Generic;
using System.Linq;

namespace AlphaKop.Core.Services.TextMatching {
    public sealed class TextMatching : ITextMatching {
        private readonly int minScore = 80;
        private readonly IRatioScorer scorer = ScorerCache.Get<WeightedRatioScorer>();

        public IEnumerable<ExtractedResult<string>> ExtractAll(string query, IEnumerable<string> choices) {
            var results = Process.ExtractSorted(
                query: query,
                choices: choices,
                scorer: scorer,
                cutoff: minScore
            );

            if (results == null) { return new ExtractedResult<string>[] { }; }

            return results
                .Select(Convert);
        }

        public ExtractedResult<string>? ExtractOne(string query, IEnumerable<string> choices) {
            var result = Process.ExtractOne(
                query: query,
                choices: choices,
                scorer: scorer,
                cutoff: minScore
            );

            if (result == null) { return null; }

            return Convert<string>(fuzzyResult: result);
        }

        private ExtractedResult<T> Convert<T>(FuzzySharp.Extractor.ExtractedResult<T> fuzzyResult) {
            return new ExtractedResult<T>(
                value: fuzzyResult.Value,
                score: fuzzyResult.Score,
                index: fuzzyResult.Index
            );
        }
    }
}