
using System;
using System.Collections.Generic;

namespace AlphaKop.Core.Services.TextMatching {
    public interface ITextMatching {
        public IEnumerable<ExtractedResult<string>> ExtractAll(string query, IEnumerable<string> choices);
        public ExtractedResult<string>? ExtractOne(string query, IEnumerable<string> choices);
    }
}
