using System.Collections.Generic;
using MyCompiler.Core.Models.GraphModels;

namespace MyCompiler.Core.Interfaces
{
    public interface ISyntacticAnalyzer<T>
    {
        IGraph Check(IEnumerable<IToken<T>> tokens);
    }
}