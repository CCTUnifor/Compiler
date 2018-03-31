namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class NonTerminal
    {
        public char Value { get; private set; }
        public NonTerminal(char value) => Value = value;
        public override string ToString() => $"{Value}";
    }
}