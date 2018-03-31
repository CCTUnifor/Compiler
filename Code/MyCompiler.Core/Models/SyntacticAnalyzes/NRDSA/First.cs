using System.Collections.Generic;
using System.Linq;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class First
    {
        public NonTerminal NonTerminal { get; }
        public ICollection<Terminal> Terminals { get; private set; }

        public First(NonTerminal nonTerminal, ICollection<Terminal> terminals)
        {
            NonTerminal = nonTerminal;
            Terminals = terminals;
        }

        public override string ToString() => $"first({NonTerminal}) => [{string.Join(", ", Terminals)}]";

        public void AddTerminal(Terminal terminal)
        {
            if (!Enumerable.Contains<string>(Terminals.Select(x => x.Value), terminal.Value))
                Terminals.Add(terminal);
        }

        public void AddTerminal(ICollection<Terminal> firstTerminals)
        {
            foreach (var terminal in firstTerminals)
                AddTerminal(terminal);
        }
    }
}