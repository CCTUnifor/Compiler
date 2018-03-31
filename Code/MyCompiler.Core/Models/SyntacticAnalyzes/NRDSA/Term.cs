namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class Term
    {
        public NonTerminal Caller { get; }
        public string[] Derivations { get; }

        public Term(NonTerminal caller, string called)
        {
            Caller = caller;
            Derivations = called.Split("|");
        }

        public override string ToString() => $"{Caller} -> {string.Join(" | ", Derivations)}";

    }
}