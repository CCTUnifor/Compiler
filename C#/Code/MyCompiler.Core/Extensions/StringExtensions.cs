using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Models.LexicalAnalyzer;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsLetter(this string value) => value.All(char.IsLetter);

        public static bool IsDigit(this string value)
            => MathExpressionLexicalAnalyzer.Digits.Contains(value);

        public static bool IsOperation(this string value)
            => MathExpressionLexicalAnalyzer.Operations.Contains(value);

        public static bool IsParentheses(this string value)
            => MathExpressionLexicalAnalyzer.Parentheses.Contains(value);

        //public static bool IsTerminal(this string value)
        //    => RegularExpressionLexicalAnalyzer.Terminal.Contains(value);

        public static bool IsRepeat(this string value)
            => RegularExpressionLexicalAnalyzer.Repeat.Contains(value);

        public static bool IsPlus(this string value)
            => RegularExpressionLexicalAnalyzer.Plus.Contains(value);

        public static bool IsOr(this string value)
            => RegularExpressionLexicalAnalyzer.Or.Contains(value);

        public static string[] GetLines(this string value)
            => value.IgnoreNewLineInWindows().Split("\n").ToArray();

        public static string IgnoreNewLineInWindows(this string value)
            => value.Replace("\r", "");

        public static string[] IgnoreEmptyOrNull(this string[] values)
            => values.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

        public static string[] GetProductions(this string value)
            => value.Split("|").Select(x => x.Trim()).ToArray();

        public static IEnumerable<Token> RemoveSpacesTokens(this IEnumerable<Token> tokens)
        {
            var x = tokens.ToList();
            x.RemoveAll(y => y == SpaceToken.Create());
            return x;
        }
    }
}

