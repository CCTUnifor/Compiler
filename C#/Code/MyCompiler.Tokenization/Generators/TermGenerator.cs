using System.Collections.Generic;
using System.Linq;
using MyCompiler.Grammar;
using MyCompiler.Grammar.Extensions;
using MyCompiler.Grammar.Tokens;
using MyCompiler.Parser;
using MyCompiler.Tokenization.Extensions;

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
                    var lex = new TopDownParser(nonTerminals, production);
                    var tokens = new List<Token>();

                    var token = lex.GetToken();
                    while (token != null)
                    {
                        tokens.Add(token);
                        token = lex.GetToken();
                    }

                    p.Add(new Production(tokens));
                }

                terms.Add(new Term(caller, p));
            }

            Terms = terms;
            return terms;
        }

        public ICollection<TerminalToken> CalculateTerminals(IEnumerable<Term> terms)
        {
            var terminals = terms.SelectMany(x => x.Productions).SelectMany(x => x.Elements).OfType<TerminalToken>().ToList();
            terminals.Add(new TerminalToken("$"));
            TerminalTokens = terminals;
            return terminals;
        }
    }
}