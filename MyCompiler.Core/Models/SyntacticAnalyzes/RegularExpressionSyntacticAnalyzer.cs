using System;
using System.Collections.Generic;
using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Core.Interfaces;

namespace MyCompiler.Core.Models.SyntacticAnalyzes
{
    public class RegularExpressionSyntacticAnalyzer : ISyntacticAnalyzer<RegularExpressionGrammarClass>
    {
        private RegularExpressionToken Token;
        private RegularExpressionParserToken<RegularExpressionGrammarClass> _regexParser;

        public void Check(IEnumerable<IToken<RegularExpressionGrammarClass>> tokens)
        {
            _regexParser = new RegularExpressionParserToken<RegularExpressionGrammarClass>(tokens);

            if (_regexParser.More())
                RE();
        }

        private RegEx RE()
        {
            var term = Termo();

            if (!_regexParser.More() || _regexParser.Peek().Value != "|") return term;

            _regexParser.Eat(_regexParser.Peek());
            var regex = RE();
            return new Choice(term, regex);

        }

        private RegEx Termo()
        {
            //var factor = RegEx.
            throw new NotImplementedException();
        }
    }    
}
