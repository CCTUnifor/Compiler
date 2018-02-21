using System;
using System.Collections.Generic;
using MyCompiler.Core.Enums;
using MyCompiler.Core.Extensions;
using MyCompiler.Core.Interfaces;

namespace MyCompiler.Core.Models
{
    public class MathExpressionLexicalAnalyzer : ILexicalAnalyzer
    {
        public static string Parentheses => "()";
        public static string Operations => "+-/*";
        public static string Digits => "0123456789";

        public Token LastToken { get; set; }
        public bool IsToAdd { get; set; }
        public StateType CurrentState { get; set; }

        public MathExpressionLexicalAnalyzer()
        {
            CurrentState = StateType.Initial;
        }

        public IEnumerable<Token> LoadTokens(string input)
        {
            input = input.Replace(" ", "");

            var tokens = new List<Token>();
            var i = 0;

            while (i < input.Length)
            {
                var value = input[i].ToString();

                switch (CurrentState)
                {
                    case StateType.Initial:
                        HandleInitialState(value);
                        i--;
                        break;

                    case StateType.Digit:
                        if (!value.IsDigit())
                            i = GoToInitialState(i);
                        else
                            HandleState(GrammarClass.Digits, value);
                        break;

                    case StateType.Operation:
                        if (!value.IsOperation())
                            i = GoToInitialState(i);
                        else
                            HandleState(GrammarClass.Operations, value);
                        break;

                    case StateType.Parentheses:
                        if (!value.IsParentheses())
                            i = GoToInitialState(i);
                        else
                            HandleState(GrammarClass.Parentheses, value);
                        break;

                    case StateType.Final:
                        if (value.IsDigit() || value.IsParentheses() || value.IsOperation())
                            i = GoToInitialState(i);
                        else
                            HandleState(GrammarClass.IsNotGrammarClass, value);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (IsToAdd)
                    tokens.Add(LastToken);
                i++;
            }

            return tokens;
        }

        private void HandleInitialState(string value)
        {
            if (value.IsDigit())
                CurrentState = StateType.Digit;
            else if (value.IsOperation())
                CurrentState = StateType.Operation;
            else if (value.IsParentheses())
                CurrentState = StateType.Parentheses;
            else
                CurrentState = StateType.Final;
        }

        private int GoToInitialState(int i)
        {
            CurrentState = StateType.Initial;
            IsToAdd = false;
            i--;
            return i;
        }

        private void HandleState(GrammarClass operation, string value)
        {
            if (LastToken == null || LastToken.GrammarClasse != operation)
                CreateToken(operation, value);
            else if (LastToken.GrammarClasse == operation)
                ConcatToken(value);
        }

        private void CreateToken(GrammarClass operation, string value)
        {
            LastToken = new Token(value, operation);
            IsToAdd = true;
        }

        private void ConcatToken(string value)
        {
            LastToken.ConcatValue(value);
            IsToAdd = false;
        }
    }
}