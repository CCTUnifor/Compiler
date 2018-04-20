using System.Collections.Generic;
using System.Linq;
using CCTUnifor.Logger;
using MyCompiler.Core.Aspects;
using MyCompiler.Core.Exceptions;
using MyCompiler.Core.Extensions;
using MyCompiler.Core.Models.Generators;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Tokenization.TopDown
{
    public class TopDownTokenization
    {
        private string Grammar { get; }

        public TermGenerator TermGenerator { get; private set; }
        public IEnumerable<NonTerminalToken> NonTerminals => TermGenerator.NonTerminalTokens;
        public IEnumerable<TerminalToken> Terminals => TermGenerator.TerminalTokens;
        public IEnumerable<Term> Terms => TermGenerator.Terms;

        public Term[,] Table { get; private set; }

        public FirstGenerator FirstGenerator { get; private set; }
        public IEnumerable<First> Firsts => FirstGenerator.Firsts;
        public FollowGenerator FollowGenerator { get; private set; }
        public IEnumerable<Follow> Follows => FollowGenerator.Follows;

        public TableGenerator TableGenerator { get; private set; }


        public TopDownTokenization(string grammar) => Grammar = grammar;

        private static int stackCounterPad = 3;
        private static int stackPad = 110;
        private static int stackInputPad = 20;
        private static int stackCalledPad = 50;

        private static void PrintHeaderStack()
            => Logger.PrintLn($"{" #".PadRight(stackCounterPad + 2)} {" Stack".PadRight(stackPad + 2)} {" Input".PadRight(stackInputPad + 2)} {" Term".PadRight(stackCalledPad + 2)}");
        private static void PrintRowStack(IEnumerable<string> q, int count, IEnumerable<string> restOfTheInput, string termString)
            => Logger.PrintLn($"[{count.ToString().PadRight(stackCounterPad)}] [{string.Join(", ", q).PadRight(stackPad)}] [{string.Join(" ", restOfTheInput).PadRight(stackInputPad)}] [{termString.PadRight(stackCalledPad)}]");


        public void Parser(string input)
        {
            HandleLines();
            Analyse(input);
        }

        private void HandleLines()
        {
            var lines = GetLines();

            TermGenerator = new TermGenerator();
            TermGenerator.CalculateNonTerminals(lines);

            TermGenerator.CalculateTerms(NonTerminals, lines);
            TermGenerator.CalculateTerminals(Terms);

            FirstGenerator = new FirstGenerator(Terms);
            FirstGenerator.GenerateFirsts();

            FollowGenerator = new FollowGenerator(Terms, Firsts);
            FollowGenerator.GenerateFollows();

            TableGenerator = new TableGenerator(Terms, NonTerminals, Terminals, Firsts, Follows);
            Table = TableGenerator.GenerateTable();
        }

        private string[] GetLines() => Grammar.GetLines().IgnoreEmptyOrNull();

        [LogAnalyserAspect]
        private void Analyse(string input)
        {
            PrintHeaderStack();

            var lines = input.Split("\n").Select(x => x.Replace("\t", "").Replace("\r", "").Trim()).ToArray();
            lines[lines.Length - 1] = lines[lines.Length - 1] + " $";

            var count = 0;
            var q = new Stack<string>();
            var X = NonTerminals.First().Value;

            q.Push("$");
            q.Push(X);

            for (var l = 0; l < lines.Length; l++)
            {
                var line = lines[l];

                var i = 0;
                var strings = line.Split(" ");

                while (X != "$" && i < strings.Length)
                {
                    var f = strings[i];
                    var M = TableGenerator.GetIndexNonTerminal(X);
                    var a = TableGenerator.GetIndexTerminal(f);

                    var restOfTheInput = strings.ToList();
                    restOfTheInput.RemoveRange(0, i);
                    var lineString = $"Line: {l + 1} | Collumn: {i + 1}\n\n";

                    if (X == f || (X == "ide" && f.IsLetter()) || (X == "num" && f.IsDigit()))
                    {
                        if (q.Peek() != X && (q.Peek() == "ide" && !X.IsLetter()) && q.Peek() == "num" && !X.IsDigit())
                            throw new CompilationException($"{lineString}Expeted: '{X}'; got '{q.Peek()}'");
                        PrintRowStack(q, count, restOfTheInput, "Next");

                        q.Pop();
                        i++;
                    }
                    else if (a >= 0 && Table[M, a] != null)
                    {
                        if (q.Peek() != X)
                            throw new CompilationException($"{lineString}Expeted: '{X}'; got '{q.Peek()}'");
                        var xc = Table[M, a];

                        PrintRowStack(q, count, restOfTheInput, xc.ToString());

                        q.Pop();

                        if (xc.Productions.Count() > 1)
                            throw new CompilationException($"{lineString}Ambiguous grammar in \n{xc}");

                        foreach (var production in xc.Productions)
                        {

                            var productionSplited = production.Elements.RemoveSpacesTokens().ToArray();
                            for (var j = productionSplited.Length - 1; j >= 0; j--)
                            {
                                if (!productionSplited[j].IsEmpty() && !string.IsNullOrEmpty(productionSplited[j].Value))
                                    q.Push(productionSplited[j].Value);
                            }
                        }
                    }
                    else
                        throw new CompilationException($"{lineString}The {f} doesn't exists in this grammar!\nStack: [{string.Join(", ", q)}] \nX: '{X}' ; f: '{f}'; \nM: '{M}'; a: '{a}';");

                    X = q.Peek();
                    count++;
                }
            }

            PrintRowStack(q, count, new List<string> { "$" }, "Accepted");

        }
    }
}