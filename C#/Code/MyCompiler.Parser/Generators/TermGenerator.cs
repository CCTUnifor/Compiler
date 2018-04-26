using System.Collections.Generic;
using System.Linq;
using MyCompiler.Grammar;
using MyCompiler.Grammar.Extensions;
using MyCompiler.Grammar.Tokens;

namespace MyCompiler.Tokenization.Generators
{
    public class TermGenerator
    {
        public IEnumerable<NonTerminalToken> NonTerminalTokens { get; private set; }
        public IEnumerable<TerminalToken> TerminalTokens { get; private set; }
        public IEnumerable<Term> Terms { get; private set; }

        public IEnumerable<NonTerminalToken> CalculateNonTerminals(IEnumerable<string> lines)
        {
            NonTerminalTokens = lines.Select(line => line.Split("->").FirstOrDefault()?.Trim().ToNonTerminal()).ToList();
            return NonTerminalTokens;
        }

        public ICollection<Term> CalculateTerms(IEnumerable<NonTerminalToken> nonTerminals, IEnumerable<string> lines)
        {
            var terms = new List<Term>();

            foreach (var line in lines)
            {
                var caller = line.Split("->").FirstOrDefault()?.Trim().ToNonTerminal();
                var called = line.Split("->").LastOrDefault()?.Trim() ?? "";

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
            var terminalTokens = selectMany.OfType<TerminalToken>().ToList();

            terminalTokens.AddRange(selectMany.OfType<NumberToken>().Select(x => x.ToTerminalToken()).Distinct().ToArray());
            terminalTokens.AddRange(selectMany.OfType<IdentifierToken>().Select(x => x.ToTerminalToken()).Distinct().ToArray());
            terminalTokens.Add(new TerminalToken("$"));

            TerminalTokens = terminalTokens;
            return terminalTokens;
        }
    }
}