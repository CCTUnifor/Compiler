namespace MyCompiler.Core.Models.Tokens
{
    public class EmptyToken : Token
    {
        public EmptyToken() : base("ε")
        {
        }

        public static TerminalToken ToTerminal()
            => new TerminalToken("ε");
    }
}