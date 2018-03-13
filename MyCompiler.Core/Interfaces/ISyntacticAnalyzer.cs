using System.Collections.Generic;

namespace MyCompiler.Core.Interfaces
{
    public interface ISyntacticAnalyzer<T>
    {
        void Check(IEnumerable<IToken<T>> tokens);
    }
}