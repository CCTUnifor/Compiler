﻿using System;
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
            var second = LexicalAnalyze.GetNextToken();
            var d = LexicalAnalyze.GetNextToken();
            throw new NotImplementedException();
        }
    }

    public class TinyLexicalAnalyze
    {
        private readonly string _input;
        private int Cursor { get; set; }
        private int NextToReturn { get; set; }
        public TinyStateType State { get; set; }
        public ICollection<TinyToken> Tokens { get; set; }
        public TinyToken LastToken => Tokens.LastOrDefault();
        private bool HasConcatanation = true;

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

        private bool ContinueLoop()
        {
            return Cursor < _input.Length && HasConcatanation;
        }

        public TinyToken GetNextToken()
        {
            NextToReturn++;
            
            HasConcatanation = true;

            while (ContinueLoop())
            {
                var character = _input[Cursor];

                switch (State)
                {
                    case TinyStateType.Initial:
                        HandleInitialState(character);
                        Cursor--;
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
                Cursor++;
            }

            if (NextToReturn - 1 >= Tokens.Count)
                return null;

            return Tokens.ToList()[NextToReturn - 1];
        }

        private void GoToInitialState()
        {
            Cursor--;
            State = TinyStateType.Initial;
            Indentifier();
            HasConcatanation = false;
        }

        private void Handle(char character, TinyGrammar grammar)
        {
            if (LastToken == null || LastToken.Grammar != grammar)
                Tokens.Add(new TinyToken(character, grammar));
            else if (LastToken.Grammar == grammar)
                LastToken.ConcatValue(character);
        }

        private void Indentifier()
        {
            if (LastToken.Grammar == TinyGrammar.Letter && ReserveWords.Contains(LastToken.Value))
                LastToken.ChangeGrammar(TinyGrammar.ReserveWord);
            else
                LastToken.ChangeGrammar(TinyGrammar.Identifier);
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
