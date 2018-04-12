using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class First : Base
    {

        public First(NonTerminalToken nonTerminal, ICollection<TerminalToken> terminals) : base(nonTerminal, terminals)
        {
        }
        public override string ToString() => $"first({NonTerminal}) => [{string.Join(", ", Terminals)}]";

        public bool AnyEmpty() => Terminals.Any(x => x.Value == "ε");
    }
}