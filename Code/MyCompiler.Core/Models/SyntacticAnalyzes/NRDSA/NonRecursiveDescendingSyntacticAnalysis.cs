using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Exceptions;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class NonRecursiveDescendingSyntacticAnalysis
    {
        private string Grammar { get; }
        private ICollection<Term> Terms { get; set; }
        private ICollection<First> Firsts { get; set; }
        private ICollection<Follow> Follows { get; set; }

        private ICollection<NonTerminal> NonTerminals { get; set; }
        private List<Terminal> Terminals { get; set; }
        private Term[,] Table { get; set; }
        private bool IsTerminal(string production) => Terminals.Any(x => x.Value == production);

        public NonRecursiveDescendingSyntacticAnalysis(string grammar) => Grammar = grammar;
        private Term GetTermByElement(char firstElement) => Terms.SingleOrDefault(x => x.Caller.Value == firstElement);
        private static void PrintRowStack(IEnumerable<string> q, int count, IEnumerable<string> restOfTheInput, string termString) => Printable.Printable.PrintLn($"[{count.ToString().PadRight(2)}] [{string.Join(", ", q).PadRight(25)}] [{string.Join(" ", restOfTheInput).PadRight(25)}] [{termString.PadRight(25)}]");

        public void Parser(string input)
        {
            GenerateTerms();
            GenerateFirst();
            GenerateFollows();
            GenerateTable();
            Analyse(input);
        }

        private void GenerateTerms()
        {
            var lines = Grammar.Split("\n").Select(x => x.Replace("\r", "")).ToArray();
            Terms = new List<Term>();
            Firsts = new List<First>();
            Follows = new List<Follow>();
            NonTerminals = new List<NonTerminal>();
            Terminals = new List<Terminal>();

            foreach (var line in lines)
            {
                var caller = line.Split("->").FirstOrDefault()?.Trim()[0].ToNonTerminal();
                var called = line.Split("->").LastOrDefault()?.Trim();

                var term = new Term(caller, called);
                Terms.Add(term);

                InitializeFirst(term);
                InitializeFollow(term);
                NonTerminals.Add(caller);
            }

            foreach (var line in lines)
            {
                var called = line.Split("->").LastOrDefault()?.Trim();
                var splited = SplitByNonTerminals(called);
                Terminals.AddRange(splited.Select(x => x.ToTerminal()).Where(x => Terminals.All(y => y.Value != x.Value)));
            }

            Terminals.Add('$'.ToTerminal());
        }

        private IEnumerable<string> SplitByNonTerminals(string called)
        {
            var c = NonTerminals.Select(x => x.Value).ToList();
            c.Add('ε');
            c.Add('|');
            c.Add(' ');

            return called.Split(c.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Distinct().ToArray();
        }

        private void GenerateFirst()
        {
            foreach (var term in Terms)
                First(term);

            PrintInfo.PrintFirsts(Firsts);
        }

        private void GenerateFollows()
        {
            Follows.First().AddFinalSymble();

            foreach (var term in Terms)
                Follow(term);

            PrintInfo.PrintFollows(Follows);
        }

        private void GenerateTable()
        {
            Table = new Term[NonTerminals.Count, Terminals.Count];
            Printable.Printable.IsToPrintInConsole = false;

            foreach (var term in Terms)
            {
                var i = NonTerminals.ToList().IndexOf(term.Caller);

                foreach (var termProduction in term.Productions)
                {
                    var production = termProduction.Trim();
                    var first = production.Split(" ").First();

                    var f = IsTerminal(first) || first == "ε"
                        ? new First(term.Caller, new List<Terminal> { first.ToTerminal() })
                        : Firsts.Single(x => x.NonTerminal.Value == first[0]);

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

            PrintInfo.PrintTable(Terminals, NonTerminals, Table);
        }

        private void Analyse(string input)
        {
            Printable.Printable.IsToPrintInConsole = true;
            Printable.Printable.PrintLn($"++++++ Analyse the input: '{input}' ++++++\n");

            var lines = input.Split("\n").Select(x => x.Replace("\t", "").Replace("\r", ""));

            foreach (var line in lines)
            {
                var X = NonTerminals.First().Value.ToString();

                var q = new Stack<string>();
                q.Push("&");
                q.Push(X);

                var i = 0;
                var count = 0;

                while (X != "&")
                {
                    var strings = line.Split(" ");
                    var f = strings[i];
                    var M = NonTerminals.Select(x => x.Value.ToString()).ToList().IndexOf(X);
                    var a = Terminals.Select(x => x.Value).ToList().IndexOf(f);

                    var restOfTheInput = strings.ToList();
                    restOfTheInput.RemoveRange(0, i);

                    if (a < 0)
                        throw new CompilationException($"The {f} doesn't exists in this grammar!");
                    if (!IsTerminal(f))
                        continue;
                    if (X == f)
                    {
                        if (q.Peek() != X)
                            throw new CompilationException($"Expeted: '{X}'; got '{q.Peek()}'");
                        q.Pop();
                        i++;
                        PrintRowStack(q, count, restOfTheInput, "Next");
                    }
                    else if (Table[M, a] != null)
                    {
                        if (q.Peek() != X)
                            throw new CompilationException($"Expeted: '{X}'; got '{q.Peek()}'");

                        q.Pop();
                        var xc = Table[M, a];
                        PrintRowStack(q, count, restOfTheInput, xc.ToString());

                        foreach (var production in xc.Productions)
                        {
                            var productionSplited = production.Split(" ");
                            for (var j = productionSplited.Length - 1; j >= 0; j--)
                            {
                                if (!productionSplited[j].IsEmpty())
                                    q.Push(productionSplited[j]);
                            }
                        }
                    }
                    else
                        throw new CompilationException($"Stack: [{string.Join(", ", q)}] \nX: '{X}' ; f: '{f}'; \nM: '{M}'; a: '{a}';");

                    X = q.Peek();
                    count++;
                }
                PrintRowStack(q, count, new List<string> { "$" }, "Accepted");
            }



            Printable.Printable.PrintLn("\nCompile success!!!!");
        }

        private First First(Term term)
        {
            var currentFirst = Firsts.Single(x => x.NonTerminal == term.Caller);

            foreach (var s in term.Productions)
            {
                var production = s.Trim();
                var firstElement = production.Split(" ").FirstOrDefault();

                if (IsTerminal(firstElement) || firstElement.IsEmpty())
                    currentFirst.AddTerminal(firstElement.ToTerminal());
                else
                {
                    var termDerivated = GetTermByElement(firstElement[0]);
                    currentFirst.AddTerminal(First(termDerivated).RemoveEmpty().Terminals);

                    var allTermsOfAllProductions = production.Select(GetTermByElement).ToList();
                    for (var i = 1; i < production.Replace(" ", "").Length; i++)
                    {
                        var X1 = GetTermByElement(production.Replace(" ", "")[i - 1]);
                        var X2 = GetTermByElement(production.Replace(" ", "")[i]);

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

        private void Follow(Term termChoosed)
        {
            var allTermsCurrent = Terms.Where(term1 => term1.Productions.Any(term1Production => term1Production.Contains(termChoosed.Caller.Value.ToString()))).ToList();

            foreach (var currentTerm in allTermsCurrent)
            {
                var productionsChosed = currentTerm.Productions.Where(x => x.Contains(termChoosed.Caller.Value));
                foreach (var y in productionsChosed)
                {
                    var production = y.Trim();
                    var i = production.IndexOf(termChoosed.Caller.Value);

                    var splited = production.Split(termChoosed.Caller.Value, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

                    var aBb = splited.Length > 1;
                    var aB = splited.Length > 0;

                    var followB = Follows.Single(x => x.NonTerminal.Value == production[i]);
                    var followA = Follows.Single(x => x.NonTerminal == currentTerm.Caller);

                    if (aBb)
                    {
                        for (int j = 1; j < splited.Length; j++)
                        {
                            var firstb = Firsts.SingleOrDefault(x => x.NonTerminal.Value == splited.Last()[0]);
                            var c = splited[j];
                            var v = c.Split(" ").First();

                            if (IsTerminal(v))
                                followB.AddTerminal(v.ToTerminal());
                            else
                                followB.AddTerminal(firstb?.RemoveEmpty().Terminals);
                        }

                    }
                    else if (aB)
                        followB.AddTerminal(followA.Terminals);
                }
            }
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
    }
}