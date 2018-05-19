using System.Collections.Generic;
using MyCompiler.Core.Interfaces.Graph;

namespace MyCompiler.Core.Interfaces.Parsers
{
    public interface IParser
    {
        void Parser(string input);
    }
}