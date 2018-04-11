namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class NonTerminal
    {
        public string Value { get; private set; }
        public NonTerminal(string value) => Value = value;
        public override string ToString() => $"{Value}";
    }
}