using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCompiler.TinyApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write your Input: ");
            var input = Console.ReadLine();

            var tiny = new TinySyntacticAnalyzer(input);

            tiny.Check();
        }
    }

    public class TinySyntacticAnalyzer
    {
        private readonly string _input;
        public TinyLexicalAnalyze LexicalAnalyze { get; set; }

        public TinySyntacticAnalyzer(string input)
        {
            _input = input;
            LexicalAnalyze = new TinyLexicalAnalyze(_input);
        }

        public void Check()
        {
            var firsttoken = LexicalAnalyze.GetNextToken();
            throw new NotImplementedException();
        }
    }

    public class TinyLexicalAnalyze
    {
        private readonly string _input;
        private int i { get; set; }
        public TinyStateType State { get; set; }
        public ICollection<TinyToken> Tokens { get; set; }
        public TinyToken LastToken => Tokens.LastOrDefault();
        private bool ToStop;

        public TinyLexicalAnalyze(string input)
        {
            _input = input;
            State = TinyStateType.Initial;
            Tokens = new List<TinyToken>();
        }

        public TinyToken GetNextToken()
        {
            ToStop = false;
            while (i < _input.Length && !ToStop)
            {
                var character = _input[i];

                switch (State)
                {
                    case TinyStateType.Initial:
                        HandleInitialState(character);
                        i--;
                        break;
                    case TinyStateType.Letter:
                        if (char.IsLetter(character))
                            Handle(character, TinyGrammar.Letter);
                        else
                            GoToInitialState();
                        break;
                    case TinyStateType.Digit:
                        if (char.IsDigit(character))
                            Handle(character, TinyGrammar.Digit);
                        else
                            GoToInitialState();
                        break;
                    case TinyStateType.Final:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                i++;
            }

            return LastToken;

        }

        private void GoToInitialState()
        {
            i--;
            State = TinyStateType.Initial;
        }

        private void Handle(char character, TinyGrammar grammar)
        {
            if (LastToken != null && LastToken.Grammar == grammar)
                LastToken.ConcatValue(character);
            else
                Tokens.Add(new TinyToken(character, grammar));
            ToStop = true;
        }

        private TinyToken CreateToken()
        {
            throw new NotImplementedException();
        }

        private void HandleInitialState(char value)
        {
            if (char.IsLetter(value))
                State = TinyStateType.Letter;
            else if (char.IsDigit(value))
                State = TinyStateType.Digit;
        }
    }

    public enum TinyStateType
    {
        Initial,
        Final,
        Letter,
        Digit
    }

    public class TinyToken
    {
        private string _value;
        public TinyGrammar Grammar { get; private set; }

        public TinyToken(char value, TinyGrammar grammar)
        {
            _value = value.ToString();
            Grammar = grammar;
        }

        public void ConcatValue(char character)
            => _value += character.ToString();
    }

    public enum TinyGrammar
    {
        Letter,
        Digit
    }
}
