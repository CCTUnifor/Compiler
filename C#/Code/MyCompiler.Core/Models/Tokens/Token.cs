namespace MyCompiler.Core.Models.Tokens
{
    public abstract class Token
    {
        public string Value { get; private set; }

        protected Token(string value)
        {
            Value = value;
        }

        public bool IsEmpty()
        {
            return Value == "ε";
        }

        public override string ToString() => $"{Value}";

        public TerminalToken ToTerminalToken() => new TerminalToken(Value);
    }
}