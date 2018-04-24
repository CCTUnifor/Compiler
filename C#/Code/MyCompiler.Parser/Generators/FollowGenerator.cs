using System.Collections.Generic;
using System.Linq;
using MyCompiler.Grammar;
using MyCompiler.Grammar.Extensions;
using MyCompiler.Grammar.Tokens;
using MyCompiler.Parser.TopDown;
using MyCompiler.Tokenization.Aspects;

namespace MyCompiler.Parser.Generators
{
    public class FollowGenerator
    {
        private readonly IEnumerable<Term> _terms;
        private readonly IEnumerable<First> _firsts;
        public ICollection<Follow> Follows { get; private set; }
        public FollowGenerator(IEnumerable<Term> terms, IEnumerable<First> firsts)
        {
            _terms = terms;
            _firsts = firsts;
        }

        [LogFollowAspect]
        public ICollection<Follow> GenerateFollows()
        {
            Initial();

            Follows.First().AddFinalSymble();

            foreach (var term in _terms)
                GenerateFollow(term);

            return Follows;
        }

        private void Initial()
        {
            Follows = new List<Follow>();
            foreach (var term in _terms)
            {
                if (Follows.All(x => x.NonTerminal != term.Caller))
                    Follows.Add(new Follow(term.Caller, new List<TerminalToken>()));
            }
        }

        private void GenerateFollow(Term termChoosed)
        {
            var followB = GetFollow(termChoosed);
            var allOcurrency = GetAllOcurrency(termChoosed);

            foreach (var currentTerm in allOcurrency)
            {
                var productionsChosed = currentTerm.Productions.Where(x => x.Elements.Any(y => y == termChoosed.Caller)).ToArray();

                foreach (var production in productionsChosed)
                {
                    var elements = production.Elements.RemoveSpacesTokens().ToList();
                    for (var i = 0; i < elements.Count; i++)
                    {
                        var element = elements[i];
                        if (element != termChoosed.Caller) continue;

                        var aBb = i + 1 < elements.Count;
                        var aB = i < elements.Count;

                        var followA = Follows.Single(x => x.NonTerminal == currentTerm.Caller);

                        if (aBb)
                        {
                            var nextElement = elements[i + 1];
                            var firstb = _firsts.SingleOrDefault(x => nextElement.Equals(x.NonTerminal));

                            if (nextElement.IsTerminal())
                                followB.AddTerminal(nextElement.ToTerminalToken());
                            else
                                followB.AddTerminal(firstb?.RemoveEmpty().Terminals);

                            if (firstb?.AnyEmpty() ?? false)
                                followB.AddTerminal(followA.Terminals);

                        }
                        else if (aB)
                            followB.AddTerminal(followA.Terminals);
                    }
                }
            }
        }

        private Term[] GetAllOcurrency(Term termChoosed)
            => _terms.Where(x => x.Productions
                                 .SelectMany(y => y.Elements)
                                 .Any(y => Equals(y, termChoosed.Caller))).ToArray();

        private Follow GetFollow(Term termChoosed)
            => Follows.Single(x => x.NonTerminal.Value == termChoosed.Caller.Value);
    }
}