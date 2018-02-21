using System.Collections.Generic;
using MyCompiler.Core.Models;

namespace MyCompiler.Core.Interfaces
{
    public interface ILexicalAnalyzer
    {
        IEnumerable<Token> LoadTokens(string input);
    }
}