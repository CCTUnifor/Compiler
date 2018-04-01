using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Exceptions;
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
        private bool IsTerminal(string production) => Terminals.Any(x => x.Value == production);

        public NonRecursiveDescendingSyntacticAnalysis(string grammar) => Grammar = grammar;

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

        private string[] SplitByNonTerminals(string called)
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

            PrintTable();
        }



        private void Analyse(string input)
        {
            Printable.Printable.IsToPrintInConsole = true;
            Printable.Printable.PrintLn($"++++++ Analyse the input: '{input}' ++++++\n");

            var X = NonTerminals.First().Value.ToString();

            var q = new Stack<string>();
            q.Push("&");
            q.Push(X);

            var i = 0;
            var count = 0;

            while (X != "&")
            {
                var strings = input.Split(" ");
                var f = strings[i];
                var M = NonTerminals.Select(x => x.Value.ToString()).ToList().IndexOf(X);
                var a = Terminals.Select(x => x.Value).ToList().IndexOf(f);

                var restOfTheInput = strings.ToList();
                restOfTheInput.RemoveRange(0, i);


                if (a < 0)
                    throw new CompilationException($"The {f} doesn't exists in this grammar!");

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
            Printable.Printable.PrintLn("\nCompile success!!!!");
        }

        private static void PrintRowStack(Stack<string> q, int count, List<string> restOfTheInput, string termString)
        {
            Printable.Printable.PrintLn($"[{count.ToString().PadRight(2)}] [{string.Join(", ", q).PadRight(25)}] [{string.Join(" ", restOfTheInput).PadRight(25)}] [{termString.PadRight(25)}]");
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
            Printable.Printable.PrintLn("++++++ Firsts ++++++\n");
            foreach (var first in Firsts)
                Printable.Printable.PrintLn(first);
            Printable.Printable.PrintLn("\n");
        }

        private void PrintFollows()
        {
            Printable.Printable.PrintLn("++++++ Follows ++++++\n");
            foreach (var follow in Follows)
                Printable.Printable.PrintLn(follow);
            Printable.Printable.PrintLn("\n");
        }

        private void PrintTable()
        {
            Printable.Printable.PrintLn("++++++ Table ++++++\n");

            var collumnsHeader = Terminals.Select(x => x.Value).ToArray();
            var rowsHeader = NonTerminals.Select(x => x.Value.ToString()).ToArray();

            var tab = new ConsoleTable.ConsoleTable(collumnsHeader, rowsHeader);
            for (var i = 0; i < NonTerminals.Count; i++)
            {
                var zxc = new List<Term>();
                for (var j = 0; j < Terminals.Count; j++)
                {
                    zxc.Add(Table[i, j]);
                }
                tab.AddRow(zxc.Select(x => x?.ToString() ?? "").ToArray());
            }

            tab.Write();

            Printable.Printable.PrintLn("\n");
        }
    }
}