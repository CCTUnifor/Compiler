using System.Collections.Generic;
using MyCompiler.Core.Aspects;
using MyCompiler.Core.Extensions;
using MyCompiler.Core.Models.Generators;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class NonRecursiveDescendingSyntacticAnalysis
    {
        private string Grammar { get; }
        private ICollection<Term> Terms { get; set; }
        public ICollection<First> Firsts { get; private set; }
        public ICollection<Follow> Follows { get; private set; }

        public ICollection<NonTerminalToken> NonTerminals { get; private set; }
        public ICollection<TerminalToken> Terminals { get; private set; }
        public Term[,] Table { get; private set; }
        //private bool IsTerminal(string production) => Terminals.Any(x => x.Value == production);

        public NonRecursiveDescendingSyntacticAnalysis(string grammar) => Grammar = grammar;

        //private static int stackCounterPad = 3;
        //private static int stackPad = 110;
        //private static int stackInputPad = 20;
        //private static int stackCalledPad = 50;

        //private static void PrintHeaderStack()
        //    => Logger.PrintLn($"{" #".PadRight(stackCounterPad + 2)} {" Stack".PadRight(stackPad + 2)} {" Input".PadRight(stackInputPad + 2)} {" Term".PadRight(stackCalledPad + 2)}");
        //private static void PrintRowStack(IEnumerable<string> q, int count, IEnumerable<string> restOfTheInput, string termString)
        //    => Logger.PrintLn($"[{count.ToString().PadRight(stackCounterPad)}] [{string.Join(", ", q).PadRight(stackPad)}] [{string.Join(" ", restOfTheInput).PadRight(stackInputPad)}] [{termString.PadRight(stackCalledPad)}]");
        //private ICollection<NonTerminalToken> GetNonTerminalsOrdered() => NonTerminals.OrderByDescending(x => x.Value.Length).ToList();
        //private int GetIndexNonTerminal(string X) => NonTerminals.Select(x => x.Value).ToList().IndexOf(X);
        //private bool IsNum(string s) => s.All(char.IsDigit);
        //private bool IsLetter(string s) => s.All(char.IsLetter);

        public void Parser(string input)
        {
            HandleLines();
            Firsts = new FirstGenerator(Terms).GenerateFirsts();
            Follows = new FollowGenerator(Terms).GenerateFollows();
            GenerateTable();
            Analyse(input);
        }

        private void HandleLines()
        {
            InstantiateVariables();

            var lines = GetLines();
            NonTerminals = TermGeneator.CalculateNonTerminals(lines);
            Terms = TermGeneator.CalculateTerms(NonTerminals, lines);
            Terminals = TermGeneator.CalculateTerminals(Terms);
        }

        private void InstantiateVariables()
        {
            Terms = new List<Term>();
            Firsts = new List<First>();
            Follows = new List<Follow>();
            NonTerminals = new List<NonTerminalToken>();
            Terminals = new List<TerminalToken>();
        }

        private string[] GetLines() => Grammar.GetLines().IgnoreEmptyOrNull();


        [LogTableAspect]
        private void GenerateTable()
        {
            //Table = new Term[nonTerminals.Count, Terminals.Count];

            //foreach (var term in terms)
            //{
            //    var A = GetIndexNonTerminal(term.Caller.Value);

            //    foreach (var termProduction in term.Productions)
            //    {
            //        var production = termProduction.Trim();
            //        var first = production.Split(" ").First();

            //        var f = IsTerminal(first) || first == "ε"
            //            ? new First(term.Caller, new List<Terminal> { first.ToTerminal() })
            //            : Firsts.Single(x => x.NonTerminal.Value == first);

            //        foreach (var terminal in f.Terminals)
            //        {
            //            var a = GetIndexTerminal(terminal.Value);
            //            var t = new Term(term.Caller, production);

            //            if (A >= 0 && a >= 0)
            //                PopulateTable(A, a, t);
            //            else if (terminal.Value == "ε")
            //            {
            //                var follow = Follows.Single(x => x.NonTerminal == term.Caller);

            //                foreach (var followTerminal in follow.Terminals)
            //                {
            //                    var b = GetIndexTerminal(followTerminal.Value);
            //                    PopulateTable(A, b, t);
            //                }
            //            }
            //        }
            //    }
            //}
        }

        //private void PopulateTable(int i, int j, Term t)
        //{
        //    if (Table[i, j] == null)
        //        Table[i, j] = t;
        //    else
        //        Table[i, j].AddProduction(t.Productions);
        //}

        [LogAnalyserAspect]
        private void Analyse(string input)
        {
            //PrintHeaderStack();

            //var lines = input.Split("\n").Select(x => x.Replace("\t", "").Replace("\r", "").Trim()).ToArray();
            //lines[lines.Length - 1] = lines[lines.Length - 1] + " $";

            //var count = 0;
            //var q = new Stack<string>();
            //var X = nonTerminals.First().Value;

            //q.Push("$");
            //q.Push(X);

            //for (var l = 0; l < lines.Length; l++)
            //{
            //    var line = lines[l];

            //    var i = 0;
            //    var strings = line.Split(" ");

            //    while (X != "$" && i < strings.Length)
            //    {
            //        var f = strings[i];
            //        var M = GetIndexNonTerminal(X);
            //        var a = GetIndexTerminal(f);

            //        var restOfTheInput = strings.ToList();
            //        restOfTheInput.RemoveRange(0, i);
            //        var lineString = $"Line: {l + 1} | Collumn: {i + 1}\n\n";

            //        if (X == f || (X == "ide" && IsLetter(f)) || (X == "num" && IsNum(f)))
            //        {
            //            if (q.Peek() != X && (q.Peek() == "ide" && !IsLetter(X)) && q.Peek() == "num" && !IsNum(X))
            //                throw new CompilationException($"{lineString}Expeted: '{X}'; got '{q.Peek()}'");
            //            PrintRowStack(q, count, restOfTheInput, "Next");

            //            q.Pop();
            //            i++;
            //        }
            //        else if (a >= 0 && Table[M, a] != null)
            //        {
            //            if (q.Peek() != X)
            //                throw new CompilationException($"{lineString}Expeted: '{X}'; got '{q.Peek()}'");
            //            var xc = Table[M, a];

            //            PrintRowStack(q, count, restOfTheInput, xc.ToString());

            //            q.Pop();

            //            if (xc.Productions.Length > 1)
            //                throw new CompilationException($"{lineString}Ambiguous grammar in \n{xc}");

            //            foreach (var production in xc.Productions)
            //            {

            //                var productionSplited = production.Split(" ");
            //                for (var j = productionSplited.Length - 1; j >= 0; j--)
            //                {
            //                    if (!productionSplited[j].IsEmpty() && !string.IsNullOrEmpty(productionSplited[j]))
            //                        q.Push(productionSplited[j]);
            //                }
            //            }
            //        }
            //        else
            //            throw new CompilationException($"{lineString}The {f} doesn't exists in this grammar!\nStack: [{string.Join(", ", q)}] \nX: '{X}' ; f: '{f}'; \nM: '{M}'; a: '{a}';");

            //        X = q.Peek();
            //        count++;
            //    }
            //}

            //PrintRowStack(q, count, new List<string> { "$" }, "Accepted");

        }

        //private int GetIndexTerminal(string f)
        //{
        //    if (f == "ε")
        //        return -1;

        //    var i = Terminals.Select(x => x.Value).ToList().IndexOf(f);
        //    if (i >= 0)
        //        return i;

        //    if (IsLetter(f))
        //        return GetIndexTerminal("ide");
        //    if (IsNum(f))
        //        return GetIndexTerminal("num");

        //    return -1;
        //}
    }
}