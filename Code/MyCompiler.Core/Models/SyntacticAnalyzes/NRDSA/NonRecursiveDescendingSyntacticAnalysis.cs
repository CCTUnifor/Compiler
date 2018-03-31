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
            GenerateFollows();
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

            for (var i = Terms.Count - 1; i < Terms.Count; i--)
            {
                var term = Terms.ToList()[i];
                Follow(term);
            }

            PrintFollows();
        }

        private First First(Term term)
        {
            First f = Firsts.SingleOrDefault(x => x.NonTerminal == term.Caller);

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

        private Follow Follow(Term term)
        {
            Follow f = Follows.SingleOrDefault(x => x.NonTerminal == term.Caller);

            foreach (var y in term.Derivations)
            {
                var derivation = y.Trim();

                //if (derivation.Length == 3 && derivation[0].IsTerminal() && derivation[1].IsNonTerminal() && derivation[2].IsTerminal()) // 2
                //{
                //    var followB = Follow(Terms.SingleOrDefault(x => x.Caller.Value == derivation[1]));

                //    if (derivation[2].IsTerminal())
                //        followB.AddTerminal(new Terminal(derivation[2]));
                //    else
                //        followB.AddTerminal(Firsts.SingleOrDefault(x => x.NonTerminal.Value == derivation[2])?.Terminals);

                //}
                if (derivation.Length >= 2 && derivation[0].IsTerminal() && derivation[1].IsNonTerminal()) // 2
                {

                    var followA = Follows.SingleOrDefault(x => x.NonTerminal.Value == term.Caller.Value);
                    var followB = Follow(Terms.SingleOrDefault(x => x.Caller.Value == derivation[1]));
                    followB.AddTerminal(followA.Terminals);
                    //followB.AddTerminal(Follow());
                }

            }

            //f.AddFinalSymble();
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