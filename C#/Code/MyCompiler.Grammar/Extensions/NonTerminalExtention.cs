using System.Linq;
using MyCompiler.Grammar.Tokens;

namespace MyCompiler.Grammar.Extensions
{
    public static class NonTerminalExtention
    {
        public static NonTerminalToken ToNonTerminal(this string value) => new NonTerminalToken(value);
        public static TerminalToken ToTerminal(this string value) => new TerminalToken(value);
        public static TerminalToken ToTerminal(this char value) => new TerminalToken(value.ToString());

        public static bool IsTerminal(this char c) => !IsNonTerminal(c);
        public static bool IsNonTerminal(this char c) => char.IsLetter(c) && char.IsUpper(c);
        public static bool IsTerminal(this string c) => c.All(x => char.IsLetter(x) && char.IsLower(x));
        public static bool IsNonTerminal(this string c) => c.All(x => char.IsLetter(x) && char.IsUpper(x));
        public static bool IsEmpty(this char c) => c == 'ε';
        public static bool IsEmpty(this string c) => c == "ε";
    }
}