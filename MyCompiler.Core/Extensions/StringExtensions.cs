using MyCompiler.Core.Models;

namespace MyCompiler.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsDigit(this string value)
            => MathExpressionLexicalAnalyzer.Digits.Contains(value);

        public static bool IsOperation(this string value)
            => MathExpressionLexicalAnalyzer.Operations.Contains(value);

        public static bool IsParentheses(this string value)
            => MathExpressionLexicalAnalyzer.Parentheses.Contains(value);
    }
}
