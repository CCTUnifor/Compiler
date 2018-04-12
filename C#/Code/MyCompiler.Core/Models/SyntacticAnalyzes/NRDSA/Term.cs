using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class Term
    {
        public NonTerminalToken Caller { get; }
        public IEnumerable<Production> Productions { get; private set; }

        public Term(NonTerminalToken caller, IEnumerable<Production> productions)
        {
            Caller = caller;
            Productions = productions;
        }

        public override string ToString() => $"{Caller} -> {string.Join(" | ", Productions)}";

        public bool AnyEmptyProduction() => Productions.SelectMany(x => x.Elements).Any(y => y.IsEmpty());

        public void AddProduction(IEnumerable<Production> productions) =>
            Productions = Productions.Concat(productions).Distinct().ToArray();
    }
}