using System.Linq;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class Term
    {
        public NonTerminal Caller { get; }
        public string[] Productions { get; private set; }

        public Term(NonTerminal caller, string called)
        {
            Caller = caller;
            Productions = called.Split("|");
        }

        public override string ToString() => $"{Caller} -> {string.Join(" | ", Productions)}";

        public bool AnyEmptyProduction() => Productions.Any(x => x.Trim().IsEmpty());

        public void AddProduction(string[] tProductions) => Productions = Productions.Concat(tProductions).Distinct().ToArray();
    }

    public class Production
    {
        //public TYPE Type { get; set; }
    }
}