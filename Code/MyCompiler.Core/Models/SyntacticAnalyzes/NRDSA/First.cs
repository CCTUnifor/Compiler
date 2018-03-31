using System.Collections.Generic;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class First : Base
    {

        public First(NonTerminal nonTerminal, ICollection<Terminal> terminals) : base(nonTerminal, terminals)
        {
        }
        public override string ToString() => $"first({NonTerminal}) => [{string.Join(", ", Terminals)}]";
    }
}