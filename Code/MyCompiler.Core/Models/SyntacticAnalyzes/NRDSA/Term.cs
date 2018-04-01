using System.Linq;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class Term
    {
        public NonTerminal Caller { get; }
        public Production[] Productions { get; }

        public Term(NonTerminal caller, Production[] productions)
        {
            Caller = caller;
            Productions = productions; //called.Split("|");
        }

        public override string ToString() => $"{Caller} -> {string.Join(" | ", Productions.)}";

        public bool AnyEmptyProduction() => Productions.Any(x => x.Trim().IsEmpty());
    }

    public class Production
    {
        //public TYPE Type { get; set; }
    }
}