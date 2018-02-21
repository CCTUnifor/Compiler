using System;
using MyCompiler.Core.Enums;

namespace MyCompiler.Core.Models
{
    public class Token
    {
        public string Value { get; private set; }
        public GrammarClass GrammarClasse { get; private set; }

        public Token(string value, GrammarClass grammarClass)
        {
            Value = value;
            GrammarClasse = grammarClass;
        }

        public bool SameGramarClassTo(Token currentToken) 
            => GrammarClasse == currentToken?.GrammarClasse;

        internal void ConcatValue(string value)
            => Value += value;
    }
}