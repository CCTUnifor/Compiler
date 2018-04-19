﻿using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Aspects;
using MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Core.Models.Generators
{
    public class TableGenerator
    {
        private readonly IEnumerable<Term> _terms;
        public IEnumerable<NonTerminalToken> NonTerminals { get; private set; }
        public IEnumerable<TerminalToken> Terminals { get; private set; }
        private readonly IEnumerable<First> _firsts;
        private readonly IEnumerable<Follow> _follows;
        public Term[,] Table { get; private set; }
        public int GetIndexNonTerminal(string X) => NonTerminals.Select(x => x.Value).ToList().IndexOf(X);
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
                var A = GetIndexNonTerminal(term.Caller.Value);

                foreach (var production in term.Productions)
                {
                    var first = production.FirstToken;

                    var f = first.IsTerminal() || first.IsEmpty()
                        ? new First(term.Caller, new List<TerminalToken> { first.ToTerminalToken() })
                        : _firsts.Single(x => x.NonTerminal == first);

                    foreach (var terminal in f.Terminals)
                    {
                        var a = GetIndexTerminal(terminal.Value);
                        var t = new Term(term.Caller, production);

                        if (A >= 0 && a >= 0)
                            PopulateTable(A, a, t);
                        else if (terminal.Value == "ε")
                        {
                            var follow = _follows.Single(x => x.NonTerminal == term.Caller);

                            foreach (var followTerminal in follow.Terminals)
                            {
                                var b = GetIndexTerminal(followTerminal.Value);
                                PopulateTable(A, b, t);
                            }
                        }
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

        public int GetIndexTerminal(string f)
        {
            if (f == "ε")
                return -1;

            var i = Terminals.Select(x => x.Value).ToList().IndexOf(f);
            if (i >= 0)
                return i;

            if (IsLetter(f))
                return GetIndexTerminal("ide");
            if (IsNum(f))
                return GetIndexTerminal("num");

            return -1;
        }
    }
}