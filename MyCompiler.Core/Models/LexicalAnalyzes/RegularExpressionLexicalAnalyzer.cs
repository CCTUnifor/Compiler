using System;
using System.Collections.Generic;
using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Core.Extensions;
using MyCompiler.Core.Interfaces;

namespace MyCompiler.Core.Models.LexicalAnalyzer
{
    public class RegularExpressionLexicalAnalyzer : ILexicalAnalyzer<RegularExpressionGrammarClass>
    {
        public static string Parentheses => "()";
        public static string Repeat => "*";
        public static string Plus => "+";
        public static string Or => "|";
        public static string Terminal => "abcd";

        public IToken<RegularExpressionGrammarClass> LastToken { get; set; }
        public bool IsToAdd { get; set; }
        public RegularExpressionStateType CurrentState { get; set; }

        public RegularExpressionLexicalAnalyzer()
            => CurrentState = RegularExpressionStateType.Initial;

        public IEnumerable<IToken<RegularExpressionGrammarClass>> LoadTokens(string input)
        {
            var tokens = new List<IToken<RegularExpressionGrammarClass>>();

            var i = 0;

            while (i < input.Length)
            {
                var value = input[i].ToString();
                switch (CurrentState)
                {
                    case RegularExpressionStateType.Initial:
                        HandleInitialState(value);
                        i--;
                        break;

                    case RegularExpressionStateType.Terminal:
                        if (!value.IsTerminal())
                            i = GoInitialState(i);
                        else
                            HandleState(RegularExpressionGrammarClass.Terminal, value, i);
                        break;

                    case RegularExpressionStateType.Parentheses:
                        if (!value.IsParentheses())
                            i = GoInitialState(i);
                        else
                            HandleState(RegularExpressionGrammarClass.Parentheses, value, i);
                        break;

                    case RegularExpressionStateType.Repeat:
                        if (!value.IsRepeat())
                            i = GoInitialState(i);
                        else
                            HandleState(RegularExpressionGrammarClass.Repeat, value, i);
                        break;

                    case RegularExpressionStateType.Plus:
                        if (!value.IsPlus())
                            i = GoInitialState(i);
                        else
                            HandleState(RegularExpressionGrammarClass.Plus, value, i);
                        break;

                    case RegularExpressionStateType.Or:
                        if (!value.IsOr())
                            i = GoInitialState(i);
                        else
                            HandleState(RegularExpressionGrammarClass.Or, value, i);
                        break;

                    case RegularExpressionStateType.Final:
                        if (value.IsTerminal() || value.IsParentheses() || value.IsRepeat() || value.IsPlus() || value.IsOr())
                            i = GoInitialState(i);
                        else
                            HandleState(RegularExpressionGrammarClass.IsNotGrammarClass, value, i);
                        break;
                }
                if (IsToAdd)
                    tokens.Add(LastToken);
                i++;
            }

            PrintTokens(tokens);
            return tokens;

        }

        private void HandleInitialState(string value)
        {
            if (value.IsTerminal())
                CurrentState = RegularExpressionStateType.Terminal;
            else if (value.IsParentheses())
                CurrentState = RegularExpressionStateType.Parentheses;
            else if (value.IsRepeat())
                CurrentState = RegularExpressionStateType.Repeat;
            else if (value.IsPlus())
                CurrentState = RegularExpressionStateType.Plus;
            else if (value.IsOr())
                CurrentState = RegularExpressionStateType.Or;
            else
                CurrentState = RegularExpressionStateType.Final;
        }

        private int GoInitialState(int i)
        {
            CurrentState = RegularExpressionStateType.Initial;
            IsToAdd = false;
            i--;
            return i;
        }

        private void HandleState(RegularExpressionGrammarClass operation, string value, int line)
        {
            if (LastToken == null || LastToken.GrammarClass != operation)
                CreateToken(operation, value, line);
            else if (LastToken.GrammarClass == operation)
                ConcatToken(value);
        }

        private void CreateToken(RegularExpressionGrammarClass operation, string value, int line)
        {
            LastToken = new RegularExpressionToken(value, operation, line);
            IsToAdd = true;
        }

        private void ConcatToken(string value)
        {
            LastToken.ConcatValue(value);
            IsToAdd = false;
        }

        public void PrintTokens(IEnumerable<IToken<RegularExpressionGrammarClass>> tokens)
        {
            Console.WriteLine("\n-----------------------------------------------------\n");

            Console.WriteLine("Tokens: ");
            foreach (var token in tokens)
                Console.WriteLine($"{token.Value.PadRight(10)} - {token.GrammarClass}");

            Console.WriteLine("\n-----------------------------------------------------\n");

        }
    }
}