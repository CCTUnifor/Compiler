﻿using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Core.Interfaces.Graph;

namespace MyCompiler.Grammar.Tokens
{
    public class RegularExpressionToken : _IToken<RegularExpressionGrammarClass>
    {
        public string Value { get; private set; }
        public RegularExpressionGrammarClass GrammarClass { get; private set; }
        public int Line { get; private set; }
        public int? Collumn { get; private set; }
        public static RegularExpressionToken Blank => new RegularExpressionToken(EmptyToken.EmptyValue, RegularExpressionGrammarClass.Empty, 0);

        public RegularExpressionToken(string value, RegularExpressionGrammarClass grammarClass, int line, int collumn = default)
        {
            Value = value;
            GrammarClass = grammarClass;
            Line = line;
            Collumn = collumn;
        }

        public bool SameGramarClassTo(_IToken<RegularExpressionGrammarClass> currentToken)
            => GrammarClass == currentToken?.GrammarClass;

        void _IToken<RegularExpressionGrammarClass>.ConcatValue(string value)
            => Value += value;

        public override string ToString()
            => $"'{Value}'";
    }
}