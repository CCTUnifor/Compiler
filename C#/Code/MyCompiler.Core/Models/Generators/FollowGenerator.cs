using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Aspects;
using MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Core.Models.Generators
{
    public class FollowGenerator
    {
        private readonly ICollection<Term> _terms;
        private ICollection<Follow> _follows;
        public FollowGenerator(ICollection<Term> terms)
        {
            _terms = terms;
        }

        [LogFollowAspect]
        public ICollection<Follow> GenerateFollows()
        {
            Initial();

            _follows.First().AddFinalSymble();

            foreach (var term in _terms)
                GenerateFollow(term);

            return _follows;
        }

        private void Initial()
        {
            _follows = new List<Follow>();
            foreach (var term in _terms)
            {
                if (_follows.All(x => x.NonTerminal != term.Caller))
                    _follows.Add(new Follow(term.Caller, new List<TerminalToken>()));
            }
        }

        private void GenerateFollow(Term term)
        {
            //var followB = Follows.Single(x => x.NonTerminal.Value == termChoosed.Caller.Value);
            //var allTermsCalled = (from term in terms let y = term.Productions.SelectMany(x => x.Split(" ").ToArray()).Select(x => x.Replace(" ", "")) from termProduction in y where termProduction == termChoosed.Caller.Value select term).Distinct().ToList();

            //foreach (var currentTerm in allTermsCalled)
            //{
            //    var productionsChosed = currentTerm.Productions.Where(x => x.Split(" ").Any(y => y == termChoosed.Caller.Value)).ToArray();
            //    foreach (var y in productionsChosed)
            //    {
            //        var production = y.Trim();
            //        var elements = production.Split(" ");
            //        var c = elements.ToList().IndexOf(termChoosed.Caller.Value);


            //        var splited = production.Split(termChoosed.Caller.Value, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

            //        var aBb = c + 1 < elements.Length;
            //        var aB = c < elements.Length;

            //        var followA = Follows.Single(x => x.NonTerminal == currentTerm.Caller);

            //        if (aBb)
            //        {
            //            foreach (var t in splited)
            //            {
            //                var term = t.Split(" ");
            //                var b = term.First();

            //                var firstb = Firsts.SingleOrDefault(x => x.NonTerminal.Value == b);

            //                if (IsTerminal(b))
            //                    followB.AddTerminal(b.ToTerminal());
            //                else
            //                    followB.AddTerminal(firstb?.RemoveEmpty().Terminals);

            //                if (firstb?.AnyEmpty() ?? false)
            //                    followB.AddTerminal(followA.Terminals);
            //            }
            //        }
            //        else if (aB)
            //            followB.AddTerminal(followA.Terminals);
            //    }
            //}
            throw new System.NotImplementedException();
        }


    }
}