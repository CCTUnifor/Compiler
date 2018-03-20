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
        public string[] ReserveWords => new[]
        {
            "write",
            "read",
            "if",
            "then",
            "else",
            "end",
            "repeat",
            "until"
        };


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
                    case TinyStateType.SemiColon:
                        if (IsSemiColon(character))
                            Handle(character, TinyGrammar.SemiColon);
                        else
                            GoToInitialState();
                        break;
                    case TinyStateType.OpenComment:
                        if (IsOpenComment(character))
                            Handle(character, TinyGrammar.OpenComment);
                        else
                            GoToInitialState();
                        break;
                    case TinyStateType.CloseComment:
                        if (IsCloseComment(character))
                            Handle(character, TinyGrammar.CloseComment);
                        else
                            GoToInitialState();
                        break;
                    case TinyStateType.Attribution:
                        if (IsAttribution(character))
                            Handle(character, TinyGrammar.Attribution);
                        else
                            GoToInitialState();
                        break;
                    case TinyStateType.Space:
                        if (IsSpace(character))
                            Handle(character, TinyGrammar.Space);
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
            if (grammar == TinyGrammar.Letter && ReserveWords.Contains(LastToken.Value))
                LastToken.ChangeGrammar(TinyGrammar.ReserveWord);
            else
                LastToken.ChangeGrammar(TinyGrammar.Identifier);

            ToStop = true;
        }

        private void HandleInitialState(char value)
        {
            if (IsLetter(value))
                State = TinyStateType.Letter;
            else if (IsDigit(value))
                State = TinyStateType.Digit;
            else if (IsSemiColon(value))
                State = TinyStateType.SemiColon;
            else if (IsOpenComment(value))
                State = TinyStateType.OpenComment;
            else if (IsCloseComment(value))
                State = TinyStateType.CloseComment;
            else if (IsAttribution(value))
                State = TinyStateType.Attribution;
            else if (IsSpace(value))
                State = TinyStateType.Space;
        }

        public bool IsLetter(char v) => char.IsLetter(v);
        public bool IsDigit(char v) => char.IsDigit(v);
        public bool IsSemiColon(char v) => v == ';';
        public bool IsOpenComment(char v) => v == '{';
        public bool IsCloseComment(char v) => v == '}';
        public bool IsAttribution(char v) => v == ':';
        public bool IsSpace(char v) => v == ' ';
    }

    public enum TinyStateType
    {
        Initial,
        Final,
        Letter,
        Digit,
        SemiColon,
        OpenComment,
        CloseComment,
        Attribution,
        Space
    }

    public class TinyToken
    {
        public string Value { get; private set; }

        public TinyGrammar Grammar { get; private set; }

        public TinyToken(char value, TinyGrammar grammar)
        {
            Value = value.ToString();
            Grammar = grammar;
        }

        public void ConcatValue(char character)
            => Value += character.ToString();

        public void ChangeGrammar(TinyGrammar newGrammarType)
            => Grammar = newGrammarType;
    }

    public enum TinyGrammar
    {
        Letter,
        Digit,
        SemiColon,
        OpenComment,
        CloseComment,
        Attribution,
        Space,
        ReserveWord,
        Identifier
    }
}
