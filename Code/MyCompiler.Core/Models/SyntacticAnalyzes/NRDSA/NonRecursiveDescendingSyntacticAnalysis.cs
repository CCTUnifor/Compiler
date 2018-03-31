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
        public ICollection<Follow> Follows { get; private set; }

        public NonRecursiveDescendingSyntacticAnalysis(string grammar)
        {
            Grammar = grammar;
            Terms = new List<Term>();
            Firsts = new List<First>();
            Follows = new List<Follow>();
        }

        public void Parser()
        {
            GenerateTerms();
            GenerateFirst();
            //GenerateFollows();
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
                InitializeFollow(term);
            }
        }

        private void GenerateFirst()
        {
            foreach (var term in Terms)
                First(term);

            PrintFirsts();
        }

        private void GenerateFollows()
        {
            Follows.First().AddFinalSymble();

            //for (var i = Terms.Count - 1; i < Terms.Count; i--)
            //{
            //    var term = Terms.ToList()[i];
            //    Follow(term);
            //}
            foreach (var term in Terms)
                Follow(term);

            PrintFollows();
        }

        private First First(Term term)
        {
            var f = Firsts.SingleOrDefault(x => x.NonTerminal == term.Caller);

            foreach (var s in term.Productions)
            {
                var nonTerminas = s.Trim();
                var firstElement = nonTerminas.FirstOrDefault();

                if (nonTerminas.IsTerminal())
                    f = AddFirst(term, nonTerminas);
                else
                if (firstElement.IsTerminal() || firstElement.IsEmpty())
                    f = AddFirst(term, firstElement.ToString());
                else
                {
                    var termDerivated = GetTermByElement(firstElement);
                    f.AddTerminal(First(termDerivated).RemoveEmpty().Terminals);

                    var c = nonTerminas.Select(GetTermByElement).ToList();
                    for (var i = 1; i < nonTerminas.Length; i++)
                    {
                        var X1 = GetTermByElement(nonTerminas[i - 1]);
                        var X2 = GetTermByElement(nonTerminas[i]);

                        if (X1.AnyEmptyProduction())
                        {
                            if (X2 != null)
                                f.AddTerminal(First(X2).RemoveEmpty().Terminals);
                        }
                    }
                    if (c.All(x => x?.AnyEmptyProduction() ?? false))
                        f.AddTerminal("ε".ToTerminal());
                }
            }

            return f;
        }

        private Term GetTermByElement(char firstElement) => Terms.SingleOrDefault(x => x.Caller.Value == firstElement);

        private Follow Follow(Term term)
        {
            Follow f = Follows.SingleOrDefault(x => x.NonTerminal == term.Caller);

            foreach (var y in term.Productions)
            {
                var production = y.Trim();

                if (production.Length >= 2 && production[0].IsTerminal() && production[1].IsNonTerminal())
                {
                    var termB = Terms.SingleOrDefault(x => x.Caller.Value == production[1]);
                    var followB = Follows.SingleOrDefault(x => x.NonTerminal == termB.Caller);


                    if (production[2].IsTerminal())
                        followB.AddTerminal(new Terminal(production[2]));
                    else
                        followB.AddTerminal(Firsts.SingleOrDefault(x => x.NonTerminal.Value == production[2])?.Terminals);
                    //followB.AddTerminal(GetFirst());
                }

            }

            f.Terminals.Remove("ε".ToTerminal());
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

        private void InitializeFollow(Term term)
        {
            if (Follows.All(x => x.NonTerminal != term.Caller))
                Follows.Add(new Follow(term.Caller, new List<Terminal>()));
        }

        private void PrintFirsts()
        {
            Console.WriteLine("++++++ Firsts ++++++\n");
            foreach (var first in Firsts)
                Console.WriteLine(first);
            Console.WriteLine("\n");
        }

        private void PrintFollows()
        {
            Console.WriteLine("++++++ Follows ++++++\n");
            foreach (var follow in Follows)
                Console.WriteLine(follow);
            Console.WriteLine("\n");
        }
    }
}