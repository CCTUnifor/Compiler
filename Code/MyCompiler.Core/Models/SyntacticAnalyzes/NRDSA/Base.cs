using System.Collections.Generic;
using System.Linq;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class Base
    {
        public NonTerminal NonTerminal { get; }
        public ICollection<Terminal> Terminals { get; private set; }

        public Base(NonTerminal nonTerminal, ICollection<Terminal> terminals)
        {
            NonTerminal = nonTerminal;
            Terminals = terminals;
        }


        public void AddTerminal(Terminal terminal)
        {
            if (!Enumerable.Contains<string>(Terminals.Select(x => x.Value), terminal.Value))
                Terminals.Add(terminal);
        }

        public void AddTerminal(ICollection<Terminal> firstTerminals)
        {
            if (firstTerminals == null) return;
            foreach (var terminal in firstTerminals)
                AddTerminal(terminal);
        }

        public Term ToTerm() => new Term(NonTerminal, string.Join(" | ", Terminals));
    }
}