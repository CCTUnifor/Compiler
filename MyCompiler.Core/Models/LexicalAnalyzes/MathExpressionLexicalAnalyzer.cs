using System;
using System.Collections.Generic;
using MyCompiler.Core.Enums;
using MyCompiler.Core.Enums.MathExpression;
using MyCompiler.Core.Extensions;
using MyCompiler.Core.Interfaces;
using MyCompiler.Core.Models.MathExpression;

namespace MyCompiler.Core.Models.LexicalAnalyzer
{
    public class MathExpressionLexicalAnalyzer : ILexicalAnalyzer<MathExpressionGrammarClass>
    {
        public static string Parentheses => "()";
        public static string Operations => "+-/*";
        public static string Digits => "0123456789";

        public IToken<MathExpressionGrammarClass> LastToken { get; set; }
        public bool IsToAdd { get; set; }
        public MathExpressionStateType CurrentState { get; set; }

        public MathExpressionLexicalAnalyzer()
        {
            CurrentState = MathExpressionStateType.Initial;
        }

        public IEnumerable<IToken<MathExpressionGrammarClass>> LoadTokens(string input)
        {
            input = input.Replace(" ", "");

            var tokens = new List<IToken<MathExpressionGrammarClass>>();
            var i = 0;

            while (i < input.Length)
            {
                var value = input[i].ToString();

                switch (CurrentState)
                {
                    case MathExpressionStateType.Initial:
                        HandleInitialState(value);
                        i--;
                        break;

                    case MathExpressionStateType.Digit:
                        if (!value.IsDigit())
                            i = GoToInitialState(i);
                        else
                            HandleState(MathExpressionGrammarClass.Digits, value, i);
                        break;

                    case MathExpressionStateType.Operation:
                        if (!value.IsOperation())
                            i = GoToInitialState(i);
                        else
                            HandleState(MathExpressionGrammarClass.Operations, value, i);
                        break;

                    case MathExpressionStateType.Parentheses:
                        if (!value.IsParentheses())
                            i = GoToInitialState(i);
                        else
                            HandleState(MathExpressionGrammarClass.Parentheses, value, i);
                        break;

                    case MathExpressionStateType.Final:
                        if (value.IsDigit() || value.IsParentheses() || value.IsOperation())
                            i = GoToInitialState(i);
                        else
                            HandleState(MathExpressionGrammarClass.IsNotGrammarClass, value, i);
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
                CurrentState = MathExpressionStateType.Digit;
            else if (value.IsOperation())
                CurrentState = MathExpressionStateType.Operation;
            else if (value.IsParentheses())
                CurrentState = MathExpressionStateType.Parentheses;
            else
                CurrentState = MathExpressionStateType.Final;
        }

        private int GoToInitialState(int i)
        {
            CurrentState = MathExpressionStateType.Initial;
            IsToAdd = false;
            i--;
            return i;
        }

        private void HandleState(MathExpressionGrammarClass operation, string value, int line)
        {
            if (LastToken == null || LastToken.GrammarClass != operation)
                CreateToken(operation, value, line);
            else if (LastToken.GrammarClass == operation)
                ConcatToken(value);
        }

        private void CreateToken(MathExpressionGrammarClass operation, string value, int line)
        {
            LastToken = new MathExpressionToken(value, operation, line);
            IsToAdd = true;
        }

        private void ConcatToken(string value)
        {
            LastToken.ConcatValue(value);
            IsToAdd = false;
        }
    }
}