using System.Collections.Generic;
using System.Linq;
using MyCompiler.Grammar;
using MyCompiler.Grammar.Tokens;
using MyCompiler.Parser.TopDown;
using MyCompiler.Tokenization.Aspects;

namespace MyCompiler.Tokenization.Generators
{
    public class FirstGenerator
    {
        public readonly IEnumerable<Term> Terms;
        public ICollection<First> Firsts { get; private set; }

        public FirstGenerator(IEnumerable<Term> terms)
        {
            Terms = terms;
        }

        [LogFirstAspect]
        public ICollection<First> GenerateFirsts()
        {
            Initial();

            foreach (var term in Terms)
                GenerateFirst(term);
            return Firsts;
        }

        private void Initial()
        {
            Firsts = new List<First>();
            foreach (var term in Terms)
            {
                if (Firsts.All(x => x.NonTerminal != term.Caller))
                    Firsts.Add(new First(term.Caller, new List<TerminalToken>()));
            }
        }

        private First GenerateFirst(Term term)
        {
            var currentFirst = Firsts.Single(x => x.NonTerminal == term.Caller);

            foreach (var production in term.Productions)
            {
                if (production.FirstElementIsSpaceToken())
                    continue;

                if (production.FirstElementIsTerminal() || production.FirstElementIsEmptyToken())
                    currentFirst.AddTerminal(production.FirstToken.ToTerminalToken());
                else
                {
                    var termOfNonTerminal = GetTermByElement(production.FirstToken);
                    var firstsOfNonTerminal = GenerateFirst(termOfNonTerminal).RemoveEmpty().Terminals;
                    currentFirst.AddTerminal(firstsOfNonTerminal);

                    var productionElements = production.Elements.ToArray();
                    for (var i = 1; i < productionElements.Length; i++)
                    {
                        var X1 = GetTermByElement(productionElements[i - 1]);
                        var X2 = GetTermByElement(productionElements[i]);

                        if (X1?.AnyEmptyProduction() ?? false && X2 != null)
                            currentFirst.AddTerminal(GenerateFirst(X2).RemoveEmpty().Terminals);
                    }
                    var all = productionElements.Select(GetTermByElement).ToList();
                    if (all.All(x => x?.AnyEmptyProduction() ?? false))
                        currentFirst.AddTerminal(EmptyToken.ToTerminal());
                }
            }

            return currentFirst;
        }

        private Term GetTermByElement(Token firstElement) => Terms.SingleOrDefault(x => x.Caller.Value == firstElement.Value);
    }
}