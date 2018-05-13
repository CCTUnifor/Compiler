using System.Collections.Generic;
using System.Linq;
using MyCompiler.Grammar;
using MyCompiler.Grammar.Extensions;
using MyCompiler.Grammar.Tokens;
using MyCompiler.Tokenization.TopDown;

namespace MyCompiler.Parser.Generators
{
    public class TermGenerator
    {
        public IEnumerable<NonTerminalToken> NonTerminalTokens { get; private set; }
        public IEnumerable<TerminalToken> TerminalTokens { get; private set; }
        public IEnumerable<Term> Terms { get; private set; }

        public IEnumerable<NonTerminalToken> CalculateNonTerminals(IEnumerable<string> lines)
        {
            NonTerminalTokens = lines.Select(line => SeparateForPremise(line).FirstOrDefault()?.Trim().ToNonTerminal()).ToList();
            return NonTerminalTokens;
        }

        private static string[] SeparateForPremise(string line) => line.Split(new string[] { "->" }, System.StringSplitOptions.None);

        public ICollection<Term> CalculateTerms(IEnumerable<NonTerminalToken> nonTerminals, IEnumerable<string> lines)
        {
            var terms = new List<Term>();

            foreach (var line in lines)
            {
                var caller = SeparateForPremise(line).FirstOrDefault()?.Trim().ToNonTerminal();
                var called = SeparateForPremise(line).LastOrDefault()?.Trim() ?? "";

                var productions = called.GetProductions();
                var p = new List<Production>();

                foreach (var production in productions)
                {
                    var lex = new TopDownTokenization(nonTerminals, production);
                    var tokens = lex.GetAllTokens();

                    p.Add(new Production(tokens));
                }

                terms.Add(new Term(caller, p));
            }

            Terms = terms;
            return terms;
        }

        public ICollection<TerminalToken> CalculateTerminals(IEnumerable<Term> terms)
        {
            var selectMany = terms.SelectMany(x => x.Productions).SelectMany(x => x.Elements).ToArray();
            var terminalTokens = selectMany.OfType<TerminalToken>().Distinct().ToList();

            //terminalTokens.AddRange(selectMany.OfType<NumberToken>().Select(x => x.ToTerminalToken()).Distinct().ToArray());
            //terminalTokens.AddRange(selectMany.OfType<IdentifierToken>().Select(x => x.ToTerminalToken()).Distinct().ToArray());
            terminalTokens.Add(new TerminalToken("$"));

            TerminalTokens = terminalTokens;
            return terminalTokens;
        }
    }
}