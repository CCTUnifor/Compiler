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

        public ICollection<NonTerminal> NonTerminals { get; private set; }
        public List<Terminal> Terminals { get; private set; }
        public Term[,] Table { get; private set; }

        public NonRecursiveDescendingSyntacticAnalysis(string grammar)
        {
            Grammar = grammar;

        }

        public void Parser()
        {
            GenerateTerms();
            GenerateFirst();
            GenerateFollows();
            GenerateTable();
        }

        private void GenerateTerms()
        {
            var lines = Grammar.Split("\n");
            Terms = new List<Term>();
            Firsts = new List<First>();
            Follows = new List<Follow>();
            NonTerminals = new List<NonTerminal>();
            Terminals = new List<Terminal>();

            foreach (var line in lines)
            {
                var caller = line.Split("->").FirstOrDefault()?.TrimEnd()[0].ToNonTerminal();
                var called = line.Split("->").LastOrDefault()?.TrimStart();

                var term = new Term(caller, called);
                Terms.Add(term);

                InitializeFirst(term);
                InitializeFollow(term);


                NonTerminals.Add(caller);
            }

            foreach (var term in Terms)
            {
                foreach (var termProduction in term.Productions)
                {
                    var re = termProduction.Trim();
                    if (re.IsTerminal() && re != "ε")
                        Terminals.Add(re.ToTerminal());
                    else
                    {
                        var collection = from xc in re where xc.IsTerminal() && xc != ' ' && xc != 'ε' select xc.ToTerminal();
                        Terminals.AddRange(collection);
                    }
                }
            }
            Terminals.Add('$'.ToTerminal());

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

            foreach (var term in Terms)
                Follow(term);

            PrintFollows();
        }

        private void GenerateTable()
        {
            Table = new Term[NonTerminals.Count, Terminals.Count];

            foreach (var term in Terms)
            {
                var i = NonTerminals.ToList().IndexOf(term.Caller);

                foreach (var termProduction in term.Productions)
                {
                    var production = termProduction.Trim();
                    First f = null;
                    if (production.IsTerminal())
                        f = new First(term.Caller, new List<Terminal> { production.ToTerminal() });
                    else if (production[0].IsTerminal())
                        f = new First(term.Caller, new List<Terminal> { production[0].ToTerminal() });
                    else if (production.IsNonTerminal())
                        f = Firsts.Single(x => x.NonTerminal.Value == production[0]);

                    foreach (var terminal in f.Terminals)
                    {
                        var j = Terminals.ToList().IndexOf(terminal);
                        var t = new Term(term.Caller, production);
                        if (i >= 0 && j >= 0)
                            Table[i, j] = t;
                        else if (terminal.Value == "ε")
                        {
                            var follow = Follows.Single(x => x.NonTerminal == term.Caller);

                            foreach (var followTerminal in follow.Terminals)
                            {
                                var followIndex = Terminals.ToList().IndexOf(followTerminal);
                                Table[i, followIndex] = t;
                            }
                        }
                    }
                }
            }

            PrintTable();
        }

        private void PrintTable()
        {
            var tab = new ConsoleTable.ConsoleTable(Terminals.Select(x => x.Value).ToArray(), NonTerminals.Select(x => x.Value.ToString()).ToArray());
            for (var i = 0; i < NonTerminals.Count; i++)
            {
                var zxc = new List<Term>();
                for (var j = 0; j < Terminals.Count; j++)
                {
                    zxc.Add(Table[i, j]);
                }
                tab.AddRow(zxc.Select(x => x?.ToString().Replace(" ", "") ?? "").ToArray());
            }

            tab.Write();
        }

        private First First(Term term)
        {
            var currentFirst = Firsts.Single(x => x.NonTerminal == term.Caller);

            foreach (var s in term.Productions)
            {
                var production = s.Trim();
                var firstElement = production.FirstOrDefault();

                if (production.IsTerminal())
                    currentFirst.AddTerminal(production.ToTerminal());
                else
                if (firstElement.IsTerminal() || firstElement.IsEmpty())
                    currentFirst.AddTerminal(firstElement.ToTerminal());
                else
                {
                    var termDerivated = GetTermByElement(firstElement);
                    currentFirst.AddTerminal(First(termDerivated).RemoveEmpty().Terminals);

                    var allTermsOfAllProductions = production.Select(GetTermByElement).ToList();
                    for (var i = 1; i < production.Length; i++)
                    {
                        var X1 = GetTermByElement(production[i - 1]);
                        var X2 = GetTermByElement(production[i]);

                        if (X1.AnyEmptyProduction())
                        {
                            if (X2 != null)
                                currentFirst.AddTerminal(First(X2).RemoveEmpty().Terminals);
                        }
                    }
                    if (allTermsOfAllProductions.All(x => x?.AnyEmptyProduction() ?? false))
                        currentFirst.AddTerminal("ε".ToTerminal());
                }
            }

            return currentFirst;
        }

        private Term GetTermByElement(char firstElement) => Terms.SingleOrDefault(x => x.Caller.Value == firstElement);

        private Follow Follow(Term termChoosed)
        {
            var currentFollow = Follows.Single(x => x.NonTerminal == termChoosed.Caller);
            var allTermsCurrent = Terms.Where(term1 => term1.Productions.Any(term1Production => term1Production.Contains(termChoosed.Caller.Value.ToString()))).ToList();
            foreach (var currentTerm in allTermsCurrent)
            {
                var productionsChosed = currentTerm.Productions.Where(x => x.Contains(termChoosed.Caller.Value));
                foreach (var y in productionsChosed)
                {
                    var production = y.Trim();
                    var i = production.IndexOf(termChoosed.Caller.Value);

                    var followB = Follows.Single(x => x.NonTerminal.Value == production[i]);
                    var followA = Follows.Single(x => x.NonTerminal == currentTerm.Caller);

                    var aBb = i > 0 && production.Length > i + 1;
                    var aB = i > 0;

                    if (aBb) // 2 regra
                    {
                        var firstb = Firsts.SingleOrDefault(x => x.NonTerminal.Value == production[i + 1]);

                        if (production[i + 1].IsTerminal())
                            followB.AddTerminal(production[i + 1].ToTerminal());
                        else
                            followB.AddTerminal(firstb?.RemoveEmpty().Terminals);
                    }
                    if (aB) // 3
                    {
                        followB.AddTerminal(followA.Terminals);
                    }
                }
            }

            return currentFollow;
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