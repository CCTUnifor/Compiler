using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Tokenization.TopDown
{
    public class Production
    {
        public IEnumerable<Token> Elements { get; }

        public Production(IEnumerable<Token> elements)
        {
            Elements = elements;
        }

        public Token FirstToken => Elements.First();
        public bool FirstElementIsTerminal() => FirstToken is TerminalToken;
        public bool FirstElementIsEmptyToken() => FirstToken is EmptyToken;
        public bool FirstElementIsSpaceToken() => FirstToken is SpaceToken;
        public override string ToString() => string.Join("", Elements);
    }
}