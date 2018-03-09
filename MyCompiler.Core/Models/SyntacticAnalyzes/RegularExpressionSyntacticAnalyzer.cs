using System;
using System.Collections.Generic;
using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Core.Interfaces;
using MyCompiler.Core.Models.LexicalAnalyzer;

namespace MyCompiler.Core.Models.SyntacticAnalyzes
{
    public class RegularExpressionSyntacticAnalyzer : ISyntacticAnalyzer<RegularExpressionGrammarClass>
    {
        private RegularExpressionParserToken<RegularExpressionGrammarClass> _regexParser;

        public void Check(IEnumerable<IToken<RegularExpressionGrammarClass>> tokens)
        {
            _regexParser = new RegularExpressionParserToken<RegularExpressionGrammarClass>(tokens);

            if (_regexParser.More())
                Regex();
        }

        private void Regex()
        {
            if (!_regexParser.More())
                return;

            Term();

            if (_regexParser.Peek.Value == "|")
            {
                _regexParser.Next();
                Regex();
            }
        }

        private void Term()
        {
            switch (_regexParser.Peek.Value)
            {
                case "a":
                case "b":
                case "c":
                case "d":
                    Factor();
                    Term();
                    break;
            }
        }

        private void Factor()
        {
            Base();
            if (_regexParser.Peek.Value == "*")
                _regexParser.Next();
        }

        private void Base()
        {
            if (RegularExpressionLexicalAnalyzer.Terminal.Contains(_regexParser.Peek.Value))
            {
                _regexParser.Next();
                return;
            }

            if (_regexParser.Peek.Value != "(")
                throw new Exception("Invalid");

            _regexParser.Next();
            Regex();

            if (_regexParser.Peek.Value != "(")
                throw new Exception("Invalid");

            _regexParser.Next();
        }
    }
}
