using System.Collections.Generic;
using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Core.Exceptions;
using MyCompiler.Core.Extensions;
using MyCompiler.Core.Interfaces;

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
            else
                throw new CompilationException("Empty tokens!");

            if (_regexParser.More())
                throw new CompilationException($"Still has an token => {_regexParser}");

            _regexParser.PrintEvents();
        }

        private void Regex()
        {
            Term();

            if (!_regexParser.More() || _regexParser.Peek.GrammarClass != RegularExpressionGrammarClass.Or)
                return;

            _regexParser.Eat("|");
            Regex();
        }

        private void Term()
        {
            if (!_regexParser.More() || !_regexParser.Peek.Value.IsTerminal() && _regexParser.Peek.Value != "(")
                return;

            Factor();
            Term();
        }

        private void Factor()
        {
            Base();
            if (_regexParser.More() && _regexParser.Peek.GrammarClass == RegularExpressionGrammarClass.Repeat)
                _regexParser.Next();
        }

        private void Base()
        {
            if (_regexParser.Peek.Value == "(")
            {
                _regexParser.Eat("(");
                Regex();
                _regexParser.Eat(")");
            }
            else if (_regexParser.Peek.Value.IsTerminal())
                _regexParser.Next();
            else
                throw new CompilationException($"Expected: '(' or terminal; got: {_regexParser.Peek.Value}");
        }
    }
}
