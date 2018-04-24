using System.Collections.Generic;
using System.Linq;
using CCTUnifor.Logger;
using MyCompiler.Core.Exceptions;
using MyCompiler.Grammar;
using MyCompiler.Grammar.Extensions;
using MyCompiler.Grammar.Tokens;
using MyCompiler.Parser.Extensions;
using MyCompiler.Parser.Generators;
using MyCompiler.Parser.TopDown;
using MyCompiler.Tokenization.Aspects;
using MyCompiler.Tokenization.Generators;

namespace MyCompiler.Tokenization
{
    public class TopDownParser
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


        public TopDownParser(string grammar) => Grammar = grammar;

        private static int stackCounterPad = 3;
        private static int stackPad = 110;
        private static int stackInputPad = 20;
        private static int stackCalledPad = 50;

        private static void PrintHeaderStack()
            => Logger.PrintLn($"{" #".PadRight(stackCounterPad + 2)} {" Stack".PadRight(stackPad + 2)} {" Input".PadRight(stackInputPad + 2)} {" Term".PadRight(stackCalledPad + 2)}");
        private static void PrintRowStack(IEnumerable<Token> q, int count, IEnumerable<string> restOfTheInput, string termString)
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
            var q = new Stack<Token>();
            var topoPilha = (Token) NonTerminals.First();

            var finalToken = "$".ToTerminal();
            q.Push(finalToken);
            q.Push(topoPilha);

            for (var l = 0; l < lines.Length; l++)
            {
                var line = lines[l];
                var tokenize = new TopDownTokenization(NonTerminals, line);

                var i = 0;
                var strings = line.Split(" ");
                var current = tokenize.GetTokenIgnoreSpace();

                while (topoPilha != finalToken && i < strings.Length)
                {
                    var M = TableGenerator.GetIndexNonTerminal(topoPilha);
                    var a = TableGenerator.GetIndexTerminal(current); 

                    var restOfTheInput = strings.ToList();
                    restOfTheInput.RemoveRange(0, i);
                    var lineString = $"Line: {l + 1} | Collumn: {i + 1}\n\n";

                    if (topoPilha == current || (topoPilha.IsIde() && current.IsLetter()) || (topoPilha.IsDigit() && current.IsDigit()))
                    {
                        if (q.Peek() != topoPilha && (q.Peek().IsIde() && !topoPilha.IsLetter()) && q.Peek().IsDigit() && !topoPilha.IsDigit())
                            throw new CompilationException($"{lineString}Expeted: '{topoPilha}'; got '{q.Peek()}'");
                        PrintRowStack(q, count, restOfTheInput, "Next");

                        q.Pop();
                        current = tokenize.GetTokenIgnoreSpace();
                        i++;
                    }
                    else if (a >= 0 && Table[M, a] != null)
                    {
                        if (q.Peek() != topoPilha)
                            throw new CompilationException($"{lineString}Expeted: '{topoPilha}'; got '{q.Peek()}'");
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
                                    q.Push(productionSplited[j]);
                            }
                        }
                    }
                    else
                        throw new CompilationException($"{lineString}The {current} doesn't exists in this grammar!\nStack: [{string.Join(", ", q)}] \nX: '{topoPilha}' ; f: '{current}'; \nM: '{M}'; a: '{a}';");

                    topoPilha = q.Peek();
                    count++;
                }
            }

            PrintRowStack(q, count, new List<string> { "$" }, "Accepted");
            Logger.PrintLnSuccess("COMPILE SUCCESS CARAIO!!!!");
        }
    }
}