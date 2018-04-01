using System.Collections.Generic;
using System.Linq;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class Follow : Base
    {
        public Follow(NonTerminal nonTerminal, ICollection<Terminal> terminals) : base(nonTerminal, terminals)
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