using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA;

namespace MyCompiler.Core.Models.SyntacticAnalyzes
{
    public class NonRecursiveDescendingSyntacticAnalysis
    {
        public string Grammar { get; }
        public ICollection<Term> Terms { get; private set; }
        public ICollection<First> Firsts { get; private set; }

        public NonRecursiveDescendingSyntacticAnalysis(string grammar)
        {
            Grammar = grammar;
            Terms = new List<Term>();
            Firsts = new List<First>();
        }

        public void Parser()
        {
            GenerateTerms();
            GenerateFirst();
        }

        private void GenerateFirst()
        {
            foreach (var term in Terms)
                First(term);

            PrintFirsts();
        }

        private void GenerateTerms()
        {
            var lines = Grammar.Split("\n");

            foreach (var line in lines)
            {
                var caller = line.Split("->").FirstOrDefault()?.TrimEnd()[0].ToNonTerminal();
                var called = line.Split("->").LastOrDefault()?.TrimStart();

                var term = new Term(caller, called);
                Terms.Add(term);

                InitializeFirst(term);
            }
        }

        private First First(Term term)
        {
            First f = null;
            foreach (var s in term.Derivations)
            {
                var nonTerminas = s.Trim();
                var firstElement = nonTerminas.FirstOrDefault();

                if (nonTerminas.IsTerminal())
                    f = AddFirst(term, nonTerminas);
                else if (firstElement.IsTerminal() || firstElement.IsEmpty())
                    f = AddFirst(term, firstElement.ToString());
                else
                {
                    var termDerivated = Terms.SingleOrDefault(x => x.Caller.Value == firstElement);
                    f = AddFirst(term, First(termDerivated));

                    foreach (var nonTerminal in nonTerminas.Remove(0, 1))
                    {
                        termDerivated = Terms.SingleOrDefault(x => x.Caller.Value == nonTerminal);

                        if (termDerivated?.Derivations.FirstOrDefault().IsEmpty() ?? false)
                            f = AddFirst(term, First(termDerivated));
                    }
                }
            }

            return f;
        }

        private First AddFirst(Term term, First first)
        {
            var _first = Firsts.SingleOrDefault(x => x.NonTerminal == term.Caller);
            _first.AddTerminal(first.Terminals);
            return _first;
        }

        private First AddFirst(Term term, string terminal)
        {
            var first = Firsts.Single(x => x.NonTerminal == term.Caller);
            first.AddTerminal(terminal.ToTerminal());

            return first;
        }

        private void InitializeFirst(Term term)
        {
            if (Firsts.All(x => x.NonTerminal != term.Caller))
                Firsts.Add(new First(term.Caller, new List<Terminal>()));
        }

        private void PrintFirsts()
        {
            Console.WriteLine("++++++ Firsts ++++++\n");
            foreach (var first in Firsts)
                Console.WriteLine(first);
        }
    }
}