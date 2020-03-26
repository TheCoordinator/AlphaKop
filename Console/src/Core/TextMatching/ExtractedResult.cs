using System;
using System.Collections.Generic;

namespace AlphaKop.Core.Services.TextMatching {
    public struct ExtractedResult<T> : IComparable<ExtractedResult<T>> {
        public readonly T Value;
        public readonly int Score;
        public readonly int Index;

        public ExtractedResult(T value, int score) {
            Value = value;
            Score = score;
            Index = 0;
        }

        public ExtractedResult(T value, int score, int index) {
            Value = value;
            Score = score;
            Index = index;
        }

        public int CompareTo(ExtractedResult<T> other) {
            return Comparer<int>.Default.Compare(this.Score, other.Score);
        }

        public override string ToString() {
            if (typeof(T) == typeof(string)) {
                return $"string: {Value}, score: {Score}, index: {Index}";
            }
            return $"value: {Value?.ToString()}, score: {Score}, index: {Index}";
        }
    }
}