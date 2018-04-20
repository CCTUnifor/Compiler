namespace MyCompiler.Grammar.Tokens
{
    public class SpaceToken : Token
    {
        public SpaceToken() : base(" ")
        {
        }

        public static Token Create() => new SpaceToken();
    }
}