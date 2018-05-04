using System.Collections.Generic;
using System.Linq;
using CCTUnifor.Logger;
using MyCompiler.Core.Exceptions;
using MyCompiler.Core.Extensions;
using MyCompiler.Grammar;
using MyCompiler.Grammar.Extensions;
using MyCompiler.Grammar.Tokens;
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
        private static void PrintRowStack(IEnumerable<Token> q, int count, string restOfTheInput, string termString)
            => Logger.PrintLn($"[{count.ToString().PadRight(stackCounterPad)}] {q.ToConvertString().PadRight(stackPad)} [{string.Join(" ", restOfTheInput).PadRight(stackInputPad)}] [{termString.PadRight(stackCalledPad)}]");


        public void Parser(string input)
        {
            Logger.PrintLn("Parser!");
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

            var lines = input.Split('\n').Select(x => x.Replace("\t", "").Replace("\r", "").Trim()).ToArray();
            lines[lines.Length - 1] = lines[lines.Length - 1] + " $";

            var count = 0;
            var stackInput = new Stack<Token>();
            var topoPilha = (Token)NonTerminals.First();

            var finalToken = new FinalToken();
            stackInput.Push(finalToken);
            stackInput.Push(topoPilha);

            for (var lineCount = 0; lineCount < lines.Length; lineCount++)
            {
                var line = lines[lineCount];
                var tokenize = new TopDownTokenization(NonTerminals, line);
                var allTokens = tokenize.GetAllTokens().ToList();

                var collumnCount = 0;
                var current = tokenize.GetTokenIgnoreSpace();

                while (topoPilha != finalToken && current != null)
                {
                    var M = TableGenerator.GetIndexNonTerminal(topoPilha);
                    var a = TableGenerator.GetIndexTerminal(current);

                    var lineString = $"Line: {lineCount + 1} | Collumn: {collumnCount + 1}\n\n";

                    if (topoPilha == current || topoPilha.Value == "ide" && current.IsIdentifier() || topoPilha.Value == "num" && current.IsNumber())
                    {
                        PrintRowStack(stackInput, count, string.Join("", allTokens), "Next");

                        collumnCount += RemoveFirstToken(allTokens);
                        stackInput.Pop();
                        current = WalkInInput(tokenize);
                    }
                    else if (a >= 0 && Table[M, a] != null)
                    {
                        if (stackInput.Peek() != topoPilha)
                            throw new CompilationException($"{lineString}Expeted: '{topoPilha}'; got '{stackInput.Peek()}'");
                        var xc = Table[M, a];

                        PrintRowStack(stackInput, count, string.Join("", allTokens), xc.ToString());
                        stackInput.Pop();

                        if (xc.Productions.Count() > 1)
                            throw new CompilationException($"{lineString}Ambiguous grammar in \n{xc}");

                        foreach (var production in xc.Productions)
                        {
                            var productionSplited = production.Elements.RemoveSpacesTokens().ToArray();
                            for (var j = productionSplited.Length - 1; j >= 0; j--)
                            {
                                if (!productionSplited[j].IsEmpty() && !string.IsNullOrEmpty(productionSplited[j].Value))
                                    stackInput.Push(productionSplited[j]);
                            }
                        }
                    }
                    else
                        throw new CompilationException($"{lineString}The {current} doesn't exists in this grammar!\nStack: {stackInput.ToConvertString()} \nX: '{topoPilha}' ; f: '{current}'; \nM: '{M}'; a: '{a}';");

                    topoPilha = stackInput.Peek();
                    count++;
                }
            }

            PrintRowStack(stackInput, count, "$", "Accepted");
            Logger.PrintLnSuccess("COMPILE SUCCESS CARAIO!!!!");
        }

        private static Token WalkInInput(TopDownTokenization tokenize) => tokenize.GetTokenIgnoreSpace();

        private static int RemoveFirstToken(List<Token> allTokens)
        {
            var collumns = allTokens[0].Value.Length;
            allTokens.RemoveAt(0);

            while (allTokens.Any() && allTokens[0] is SpaceToken)
            {
                collumns += allTokens[0].Value.Length;
                allTokens.RemoveAt(0);
            }

            return collumns;
        }
    }
}