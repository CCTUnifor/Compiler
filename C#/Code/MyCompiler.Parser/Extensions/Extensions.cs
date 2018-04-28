namespace MyCompiler.Parser.Extensions
{
    public static class Extensions
    {

        public static bool IsDigit(this string value)
            => MathExpressionLexicalAnalyzer.Digits.Contains(value);

        public static bool IsOperation(this string value)
            => MathExpressionLexicalAnalyzer.Operations.Contains(value);

        public static bool IsParentheses(this string value)
            => MathExpressionLexicalAnalyzer.Parentheses.Contains(value);

        public static bool IsRepeat(this string value)
            => RegularExpressionLexicalAnalyzer.Repeat.Contains(value);

        public static bool IsPlus(this string value)
            => RegularExpressionLexicalAnalyzer.Plus.Contains(value);

        public static bool IsOr(this string value)
            => RegularExpressionLexicalAnalyzer.Or.Contains(value);
    }
}
