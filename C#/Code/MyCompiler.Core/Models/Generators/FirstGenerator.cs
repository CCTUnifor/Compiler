using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Aspects;
using MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Core.Models.Generators
{
    public class FirstGenerator
    {
        public readonly ICollection<Term> _terms;
        public ICollection<First> _firsts { get; private set; }

        public FirstGenerator(ICollection<Term> terms)
        {
            _terms = terms;
        }

        [LogFirstAspect]
        public ICollection<First> GenerateFirsts()
        {
            Initial();

            foreach (var term in _terms)
                GenerateFirst(term);
            return _firsts;
        }

        private void Initial()
        {
            _firsts = new List<First>();
            foreach (var term in _terms)
            {
                if (_firsts.All(x => x.NonTerminal != term.Caller))
                    _firsts.Add(new First(term.Caller, new List<TerminalToken>()));
            }
        }

        private First GenerateFirst(Term term)
        {
            var currentFirst = _firsts.Single(x => x.NonTerminal == term.Caller);

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

        private Term GetTermByElement(Token firstElement) => _terms.SingleOrDefault(x => x.Caller.Value == firstElement.Value);
    }
}