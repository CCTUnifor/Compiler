using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Aspects;
using MyCompiler.Core.Extensions;
using MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Core.Models.Generators
{
    public class FollowGenerator
    {
        private readonly ICollection<Term> _terms;
        private readonly ICollection<First> _firsts;
        public ICollection<Follow> Follows { get; private set; }
        public FollowGenerator(ICollection<Term> terms, ICollection<First> firsts)
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
            var followB = Follows.Single(x => x.NonTerminal.Value == termChoosed.Caller.Value);
            var allOcurrency = _terms.Where(x =>
                x.Productions.SelectMany(y => y.Elements).Any(y => Equals(y, termChoosed.Caller))).ToArray();

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
                            var firstb = _firsts.SingleOrDefault(x => x.NonTerminal == element);
                            if (element.IsTerminal())
                                followB.AddTerminal(element.ToTerminalToken());
                            else
                                followB.AddTerminal(firstb?.RemoveEmpty().Terminals);

                            if (firstb?.AnyEmpty() ?? false)
                                followB.AddTerminal(followA.Terminals);

                        }
                        else if (aB)
                            followB.AddTerminal(followA.Terminals);
                    }
                    //var indexOfTermChoosed = elements.IndexOf(termChoosed.Caller);
                    //elements.RemoveAll(x => x == termChoosed.Caller);

                    //var aBb = indexOfTermChoosed + 1 < elements.Count;
                    //var aB = indexOfTermChoosed < elements.Count;

                    //var followA = Follows.Single(x => x.NonTerminal == currentTerm.Caller);

                    //if (aBb)
                    //{
                    //    foreach (var element in elements)
                    //    {
                    //        //var b = term.First();

                    //        //var firstb = Firsts.SingleOrDefault(x => x.NonTerminal.Value == b);

                    //        //if (IsTerminal(b))
                    //        //    followB.AddTerminal(b.ToTerminal());
                    //        //else
                    //        //    followB.AddTerminal(firstb?.RemoveEmpty().Terminals);

                    //        //if (firstb?.AnyEmpty() ?? false)
                    //        //    followB.AddTerminal(followA.Terminals);
                    //    }
                    //}
                    //else if (aB)
                    //    followB.AddTerminal(followA.Terminals);
                }
            }
            //throw new System.NotImplementedException();
        }
    }
}