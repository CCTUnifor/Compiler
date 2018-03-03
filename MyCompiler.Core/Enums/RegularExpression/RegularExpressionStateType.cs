namespace MyCompiler.Core.Enums.RegularExpression
{
    public enum RegularExpressionStateType
    {
        Final = -1,
        Initial = 1,
        Terminal = 2,
        Parentheses = 3,
        Repeat = 4,
        Plus = 5,
        Or = 6,
    }
}