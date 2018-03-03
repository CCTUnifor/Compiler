using System.Collections.Generic;
using MyCompiler.Core.Models;

namespace MyCompiler.Core.Interfaces
{
    public interface ILexicalAnalyzer<T>
    {
        IEnumerable<IToken<T>> LoadTokens(string input);
    }
}