﻿using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Enums;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Core.Models.LexicalAnalyzes
{
    public class TinyLexicalAnalyze
    {
        private readonly int _countLine;
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

        public TinyLexicalAnalyze(int countLine, string input)
        {
            _countLine = countLine;
            _input = input;
            State = TinyStateType.Initial;
            Tokens = new List<TinyToken>();
        }

        private bool ContinueLoop()
            => Cursor < _input.Length && HasConcatanation;

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
                        {
                            GoToInitialState();
                            Indentifier();
                        }
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
                    case TinyStateType.Comment:
                        if (IsComment(character))
                            Handle(character, TinyGrammar.Comment);
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
                    case TinyStateType.Parentheses:
                        if (IsParentheses(character))
                            Handle(character, TinyGrammar.Parentheses);
                        else
                            GoToInitialState();
                        break;
                    case TinyStateType.Operator:
                        if (IsOperator(character))
                            Handle(character, TinyGrammar.Operator);
                        else
                            GoToInitialState();
                        break;
                    case TinyStateType.Prod:
                        if (IsProd(character))
                            Handle(character, TinyGrammar.Prod);
                        else
                            GoToInitialState();
                        break;
                    case TinyStateType.Sum:
                        if (IsSum(character))
                            Handle(character, TinyGrammar.Sum);
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

            if (LastToken.Grammar == TinyGrammar.Space)
                return GetNextToken();


            if (NextToReturn - 1 >= Tokens.Count)
                return null;

            Console.WriteLine(Tokens.ToList()[NextToReturn - 1]);
            return Tokens.ToList()[NextToReturn - 1];
        }

        private void GoToInitialState()
        {
            Cursor--;
            State = TinyStateType.Initial;
            
            HasConcatanation = false;
        }

        private void Handle(char character, TinyGrammar grammar)
        {
            if (LastToken == null || LastToken.Grammar != grammar)
                Tokens.Add(new TinyToken(_countLine, character, grammar));
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
            else if (IsComment(value))
                State = TinyStateType.Comment;
            else if (IsAttribution(value))
                State = TinyStateType.Attribution;
            else if (IsSpace(value))
                State = TinyStateType.Space;
            else if (IsParentheses(value))
                State = TinyStateType.Parentheses;
            else if (IsOperator(value))
                State = TinyStateType.Operator;
            else if (IsProd(value))
                State = TinyStateType.Prod;
            else if (IsSum(value))
                State = TinyStateType.Sum;
        }

        public bool IsLetter(char v) => char.IsLetter(v);
        public bool IsDigit(char v) => char.IsDigit(v);
        public bool IsSemiColon(char v) => v == ';';
        public bool IsComment(char v) => Comment.Contains(v);
        public bool IsAttribution(char v) => Attribution.Contains(v);
        public bool IsSpace(char v) => v == ' ';
        public bool IsParentheses(char v) => Parentheses.Contains(v);
        public bool IsOperator(char v) => Operator.Contains(v);
        public bool IsSum(char v) => Sum.Contains(v);
        public bool IsProd(char v) => Prod.Contains(v);

        private readonly char[] Operator = new[] { '<', '=' };
        private readonly char[] Parentheses = new[] { '(', ')' };
        private readonly char[] Sum = new[] { '+', '-' };
        private readonly char[] Comment = new[] { '{', '}' };
        private readonly char[] Attribution = new[] { ':', '=' };
        private readonly char[] Prod = new[] { '*', '/' };

        public bool Any()
            => Cursor < _input.Length;
    }
}