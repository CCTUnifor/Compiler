using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Extensions;
using MyCompiler.Core.Models.LexicalAnalyzes;
using MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Core.Models.Generators
{
    public static class TermGeneator
    {
        public static ICollection<NonTerminalToken> CalculateNonTerminals(IEnumerable<string> lines)
            => lines.Select(line => line.Split("->").FirstOrDefault()?.Trim().ToNonTerminal()).ToList();

        public static ICollection<Term> CalculateTerms(ICollection<NonTerminalToken> nonTerminals, IEnumerable<string> lines)
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
                    var lex = new LexicAnalyser(nonTerminals, production);
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

            return terms;
        }

        public static ICollection<TerminalToken> CalculateTerminals(ICollection<Term> terms)
        {
            var terminals = terms.SelectMany(x => x.Productions).SelectMany(x => x.Elements).OfType<TerminalToken>().ToList();
            terminals.Add(new TerminalToken("$"));

            return terminals;
        }
    }
}