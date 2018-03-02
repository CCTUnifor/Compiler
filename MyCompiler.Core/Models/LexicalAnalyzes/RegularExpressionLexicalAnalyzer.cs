using System;
using System.Collections.Generic;
using MyCompiler.Core.Enums;
using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Core.Extensions;
using MyCompiler.Core.Interfaces;
using MyCompiler.Core.Models.MathExpression;

namespace MyCompiler.Core.Models.LexicalAnalyzer
{
    public class RegularExpressionLexicalAnalyzer : ILexicalAnalyzer<RegularExpressionGrammarClass>
    {
        public static string Parentheses => "()";
        public static string Repeat => "*";
        public static string Plus => "+";
        public static string Or => "|";
        public static string Terminal => "abc";

        public RegularExpressionStateType CurrentState { get; set; }

        public RegularExpressionLexicalAnalyzer()
            => CurrentState = RegularExpressionStateType.Initial;

        public IEnumerable<IToken<RegularExpressionGrammarClass>> LoadTokens(string input)
        {
            var tokens = new List<MathExpressionToken>();

            var i = 0;

            while(i < input.Length) {
                switch(CurrentState) {
                    case RegularExpressionStateType.Initial:

                        break;
                }
            }

            throw new NotImplementedException();
        }
    }
}