using System.Collections.Generic;
using MyCompiler.Core.Interfaces.Graph;

namespace MyCompiler.Core.Interfaces
{
    public interface IParser<T>
    {
        IGraph Check(IEnumerable<IToken<T>> tokens);
    }
}