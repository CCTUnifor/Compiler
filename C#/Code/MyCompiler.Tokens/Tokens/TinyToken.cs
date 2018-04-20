using MyCompiler.Core.Enums;

namespace MyCompiler.Core.Models.Tokens
{
    public class TinyToken
    {
        public readonly int Line;
        public readonly int Collumn;

        public string Value { get; private set; }

        public TinyGrammar Grammar { get; private set; }

        public TinyToken(int line, int collumn, char value, TinyGrammar grammar)
        {
            Line = line;
            Collumn = collumn;
            Value = value.ToString();
            Grammar = grammar;
        }


        public void ConcatValue(char character)
            => Value += character.ToString();

        public void ChangeGrammar(TinyGrammar newGrammarType)
            => Grammar = newGrammarType;

        public override string ToString() => $"{{L: {Line} | C: {Collumn.ToString().PadRight(3)}}} {Grammar.ToString().PadRight(12)} - {Value}";
    }
}