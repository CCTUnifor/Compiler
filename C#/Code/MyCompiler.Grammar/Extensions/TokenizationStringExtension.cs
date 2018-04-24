using System.Collections.Generic;
using System.Linq;
using MyCompiler.Grammar;
using MyCompiler.Grammar.Tokens;

namespace MyCompiler.Grammar.Extensions
{
    public static class TokenizationStringExtension
    {

        public static bool IsLetter(this string value) => value.All(char.IsLetter);

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
