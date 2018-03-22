namespace MyCompiler.Core.Enums.MathExpression
{
    public enum MathExpressionStateType
    {
        Final = -1,
        Initial = 1,
        Digit = 2,
        Operation = 3,
        Parentheses = 4
    }
}