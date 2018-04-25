using System.Collections.Generic;
using System.Linq;
using MyCompiler.Grammar;
using MyCompiler.Grammar.Extensions;
using MyCompiler.Grammar.Tokens;
using MyCompiler.Parser.TopDown;
using MyCompiler.Tokenization.Aspects;

namespace MyCompiler.Tokenization.Generators
{
    public class TableGenerator
    {
        private readonly IEnumerable<Term> _terms;
        public IEnumerable<NonTerminalToken> NonTerminals { get; private set; }
        public IEnumerable<TerminalToken> Terminals { get; private set; }
        private readonly IEnumerable<First> _firsts;
        private readonly IEnumerable<Follow> _follows;
        public Term[,] Table { get; private set; }
        public int GetIndexNonTerminal(Token X) => NonTerminals.ToList().IndexOf(X.ToNonTerminalToken());
        private bool IsNum(string s) => s.All(char.IsDigit);
        private bool IsLetter(string s) => s.All(char.IsLetter);

        public TableGenerator(IEnumerable<Term> terms, IEnumerable<NonTerminalToken> nonTerminals,
            IEnumerable<TerminalToken> terminals, IEnumerable<First> firsts, IEnumerable<Follow> follows)
        {
            _terms = terms;
            NonTerminals = nonTerminals;
            Terminals = terminals;
            _firsts = firsts;
            _follows = follows;
        }

        [LogTableAspect]
        public Term[,] GenerateTable()
        {
            Table = new Term[NonTerminals.Count(), Terminals.Count()];

            foreach (var term in _terms)
            {
                var A = GetIndexNonTerminal(term.Caller);

                foreach (var production in term.Productions)
                {
                    var first = production.FirstToken;

                    var f = first.IsTerminal() || first.IsEmpty()
                        ? new First(term.Caller, new List<TerminalToken> { first.ToTerminalToken() })
                        : _firsts.Single(x => x.NonTerminal == first);

                    foreach (var terminal in f.Terminals)
                    {
                        var a = GetIndexTerminal(terminal);
                        var t = new Term(term.Caller, production);

                        if (terminal.Value == "ε")
                        {
                            var follow = _follows.Single(x => x.NonTerminal == term.Caller);

                            foreach (var followTerminal in follow.Terminals)
                            {
                                var b = GetIndexTerminal(followTerminal);
                                PopulateTable(A, b, t);
                            }
                        }
                        else if (A >= 0 && a >= 0)
                            PopulateTable(A, a, t);

                    }
                }
            }

            return Table;
        }

        private void PopulateTable(int i, int j, Term t)
        {
            if (Table[i, j] == null)
                Table[i, j] = t;
            else
                Table[i, j].AddProduction(t.Productions);
        }

        public int GetIndexTerminal(Token f)
        {
            if (f is EmptyToken)
                return -1;

            var i = Terminals.ToList().IndexOf(f.ToTerminalToken());
            if (i >= 0)
                return i;

            if (f.IsLetter())
                return GetIndexTerminal("ide".ToTerminal());
            if (f.IsDigit())
                return GetIndexTerminal("num".ToTerminal());

            return -1;
        }
    }
}