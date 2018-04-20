using System.Collections.Generic;
using System.Linq;
using MyCompiler.Grammar.Extensions;
using MyCompiler.Grammar.Tokens;

namespace MyCompiler.Tokenization.TopDown
{
    public class Base
    {
        public NonTerminalToken NonTerminal { get; }
        public ICollection<TerminalToken> Terminals { get; private set; }

        public Base(NonTerminalToken nonTerminal, ICollection<TerminalToken> terminals)
        {
            NonTerminal = nonTerminal;
            Terminals = terminals;
        }


        public void AddTerminal(TerminalToken terminal)
        {
            if (!Enumerable.Contains<string>(Terminals.Select(x => x.Value), terminal.Value))
                Terminals.Add(terminal);
        }

        public void AddTerminal(ICollection<TerminalToken> firstTerminals)
        {
            if (firstTerminals == null) return;
            foreach (var terminal in firstTerminals)
                AddTerminal(terminal);
        }

        //public Term ToTerm() => new Term(NonTerminal, string.Join(" | ", Terminals));
        public First RemoveEmpty()
        {
            var x = Terminals.ToList();
            x.Remove("ε".ToTerminal());

            var newFirst = new First(this.NonTerminal, x);
            return newFirst;
        }
    }
}