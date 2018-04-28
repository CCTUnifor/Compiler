namespace MyCompiler.Grammar.Tokens
{
    public class EmptyToken : Token
    {
        public static string EmptyValue = "ε";
        public EmptyToken() : base(EmptyValue)
        {
        }

        public static TerminalToken ToTerminal()
            => new TerminalToken(EmptyValue);
    }
}