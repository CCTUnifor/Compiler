using MyCompiler.Core.Enums;

namespace MyCompiler.Core.Models.Tokens
{
    public class TinyToken
    {
        public readonly int Line;
        public string Value { get; private set; }

        public TinyGrammar Grammar { get; private set; }

        public TinyToken(int line, char value, TinyGrammar grammar)
        {
            Line = line;
            Value = value.ToString();
            Grammar = grammar;
        }

        public void ConcatValue(char character)
            => Value += character.ToString();

        public void ChangeGrammar(TinyGrammar newGrammarType)
            => Grammar = newGrammarType;

        public override string ToString() => $"{{L: {Line}}} {Value} - {{{Grammar}}}";
    }
}