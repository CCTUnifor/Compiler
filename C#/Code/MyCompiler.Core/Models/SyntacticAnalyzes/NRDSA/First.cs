using System.Collections.Generic;
using System.Linq;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class First : Base
    {

        public First(NonTerminal nonTerminal, ICollection<Terminal> terminals) : base(nonTerminal, terminals)
        {
        }
        public override string ToString() => $"first({NonTerminal}) => [{string.Join(", ", Terminals)}]";

        public bool AnyEmpty() => Terminals.Any(x => x.Value == "ε");
    }
}