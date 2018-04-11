using System.Collections.Generic;

namespace MyCompiler.Core.Interfaces
{
    public interface ILexicalAnalyzer<T>
    {
        IEnumerable<IToken<T>> LoadTokens(string input);
        void PrintTokens(IEnumerable<IToken<T>> tokens);
    }
}