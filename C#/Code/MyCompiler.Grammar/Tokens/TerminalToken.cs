using MyCompiler.Core.Interfaces.Graph;

namespace MyCompiler.Grammar.Tokens
{
    public class TerminalToken : Token, ITerminalToken
    {
        public TerminalToken(string value) : base(value)
        {
        }
    }
}