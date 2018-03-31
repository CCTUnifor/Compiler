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

        public First RemoveEmpty()
        {
            var x = Terminals.ToList();
            x.Remove("ε".ToTerminal());

            var newFirst = new First(this.NonTerminal, x);
            return newFirst;
        }
    }
}