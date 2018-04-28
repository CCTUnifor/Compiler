using System.Collections.Generic;
using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Core.Exceptions;
using MyCompiler.Core.Interfaces;
using MyCompiler.Core.Interfaces.Graph;

namespace MyCompiler.Tokenization
{
    public class RegularExpressionSyntacticAnalyzer : IParser<RegularExpressionGrammarClass>
    {
        private RegularExpressionParserToken<RegularExpressionGrammarClass> _regexParser;

        public IGraph Check(IEnumerable<IToken<RegularExpressionGrammarClass>> tokens)
        {
            _regexParser = new RegularExpressionParserToken<RegularExpressionGrammarClass>(tokens);
            IGraph graph;

            if (_regexParser.More())
                graph = Regex();
            else
                throw new CompilationException("Empty tokens!");

            if (_regexParser.More())
                throw new CompilationException($"Still has an token => {_regexParser}");

            //_regexParser.PrintEvents();
            graph.Print();
            return graph;
        }

        private IGraph Regex()
        {
            var graph = Term();

            if (!_regexParser.More() || _regexParser.Peek.GrammarClass != RegularExpressionGrammarClass.Or)
                return graph;

            _regexParser.Eat("|");
            return graph.AddChoice(Regex()); // todo: concat two Graphs
        }

        private IGraph Term()
        {
            IGraph graph = null;
            if (!_regexParser.More() || _regexParser.Peek.GrammarClass != RegularExpressionGrammarClass.Terminal && _regexParser.Peek.Value != "(")
                return graph;

            graph = Factor();
            graph.AddSequence(Term());

            return graph;
        }

        private IGraph Factor()
        {
            var graph = Base();
            if (_regexParser.More() && _regexParser.Peek.GrammarClass == RegularExpressionGrammarClass.Repeat)
            {
                _regexParser.Eat(RegularExpressionLexicalAnalyzer.Repeat);
                graph.RepeatN();
            }
            else if (_regexParser.More() && _regexParser.Peek.GrammarClass == RegularExpressionGrammarClass.Plus)
            {
                _regexParser.Eat(RegularExpressionLexicalAnalyzer.Plus);
                graph.RepeatPlus();
            }

            return graph;
        }

        private IGraph Base()
        {
            IGraph graph;

            if (_regexParser.Peek.Value == "(")
            {
                _regexParser.Eat("(");
                graph = Regex();
                _regexParser.Eat(")");
            }
            else if (_regexParser.Peek.GrammarClass == RegularExpressionGrammarClass.Terminal)
            {
                graph = new Graph((RegularExpressionToken)_regexParser.Peek);
                _regexParser.Next();
            }
            else
                throw new CompilationException($"Expected: '(' or terminal; got: {_regexParser.Peek.Value}");

            return graph;
        }
    }
}