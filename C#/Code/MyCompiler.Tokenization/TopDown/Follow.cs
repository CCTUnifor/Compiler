using System.Collections.Generic;
using System.Linq;
using MyCompiler.Grammar.Extensions;
using MyCompiler.Grammar.Tokens;

namespace MyCompiler.Tokenization.TopDown
{
    public class Follow : Base
    {
        public Follow(NonTerminalToken nonTerminal, ICollection<TerminalToken> terminals) : base(nonTerminal, terminals)
        {
        }
        public override string ToString() => $"follow({NonTerminal}) => [{string.Join(", ", Terminals)}]";

        public void AddFinalSymble()
        {
            if (Terminals.Select(x => x.Value).All(y => y != "$"))
                Terminals.Add("$".ToTerminal());
        }
    }
}