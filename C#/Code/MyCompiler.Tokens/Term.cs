using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Tokenization.TopDown
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

        public Term(NonTerminalToken caller, Production production)
        {
            Caller = caller;
            Productions = new List<Production> { production };
        }

        public override string ToString() => $"{Caller} -> {string.Join(" | ", Productions)}";

        public bool AnyEmptyProduction() => Productions.SelectMany(x => x.Elements).Any(y => y.IsEmpty());

        public void AddProduction(IEnumerable<Production> productions) =>
            Productions = Productions.Concat(productions).Distinct().ToArray();
    }
}