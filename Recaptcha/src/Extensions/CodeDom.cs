using System.Collections.Generic;

namespace System.CodeDom.Compiler {
    public sealed class CompilerError {
        public string? ErrorText;
        public bool IsWarning;
    }

    public sealed class CompilerErrorCollection : List<CompilerError> { }
}