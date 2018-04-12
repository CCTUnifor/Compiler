namespace MyCompiler.Core.Models.Tokens
{
    public class EmptyToken : Token
    {
        public EmptyToken(string value) : base(value)
        {
        }

        public static TerminalToken CreateTerminal()
            => new TerminalToken("ε");
    }
}