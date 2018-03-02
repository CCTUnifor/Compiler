namespace MyCompiler.Core.Enums.RegularExpression
{
    public enum RegularExpressionStateType{
        Final = -1,
        Initial = 1,
        Digit = 2,
        Operation = 3,
        Parentheses = 4
    }
}