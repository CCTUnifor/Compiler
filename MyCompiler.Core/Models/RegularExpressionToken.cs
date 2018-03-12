using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Core.Interfaces;

namespace MyCompiler.Core.Models
{
    public class RegularExpressionToken : IToken<RegularExpressionGrammarClass>
    {
        public string Value { get; private set; }
        public RegularExpressionGrammarClass GrammarClass { get; private set; }
        public int Line { get; private set; }
        public int? Collumn { get; private set; }

        public RegularExpressionToken(string value, RegularExpressionGrammarClass grammarClass, int line, int collumn = default)
        {
            Value = value;
            GrammarClass = grammarClass;
            Line = line;
            Collumn = collumn;
        }

        public bool SameGramarClassTo(IToken<RegularExpressionGrammarClass> currentToken)
            => GrammarClass == currentToken?.GrammarClass;

        void IToken<RegularExpressionGrammarClass>.ConcatValue(string value)
            => Value += value;

        public override string ToString()
            => $"{Value} - {GrammarClass}";
    }
}