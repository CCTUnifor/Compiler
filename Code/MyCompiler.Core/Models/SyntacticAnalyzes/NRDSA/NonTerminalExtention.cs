using System.Linq;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public static class NonTerminalExtention
    {
        public static NonTerminal ToNonTerminal(this string value) => new NonTerminal(value);
        public static Terminal ToTerminal(this string value) => new Terminal(value);
        public static Terminal ToTerminal(this char value) => new Terminal(value);

        public static bool IsTerminal(this char c) => !IsNonTerminal(c);
        public static bool IsNonTerminal(this char c) => char.IsLetter(c) && char.IsUpper(c);
        public static bool IsTerminal(this string c) => c.All(x => char.IsLetter(x) && char.IsLower(x));
        public static bool IsNonTerminal(this string c) => c.All(x => char.IsLetter(x) && char.IsUpper(x));
        public static bool IsEmpty(this char c) => c == 'ε';
        public static bool IsEmpty(this string c) => c == "ε";
    }
}