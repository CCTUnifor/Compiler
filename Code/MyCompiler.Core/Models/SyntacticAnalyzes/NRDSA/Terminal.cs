namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class Terminal
    {
        public string Value { get; }
        public Terminal(char value) => Value = value.ToString();
        public Terminal(string value) => Value = value;
        public override string ToString() => $"{Value}";
    }
}