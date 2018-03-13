using System.Collections.Generic;
using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Core.Exceptions;
using MyCompiler.Core.Extensions;
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
            else
                throw new CompilationException("Empty tokens!");

            if (_regexParser.More())
                throw new CompilationException($"Still has an token => {_regexParser}");

            _regexParser.PrintEvents();
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
            if (!_regexParser.More() || !_regexParser.Peek.Value.IsTerminal() && _regexParser.Peek.Value != "(")
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
            else if (_regexParser.Peek.Value.IsTerminal())
            {
                graph = new Graph();
                _regexParser.Next();
            }
            else
                throw new CompilationException($"Expected: '(' or terminal; got: {_regexParser.Peek.Value}");

            return graph;
        }
    }

    public interface IGraph
    {
        void AddSequence(IGraph sequenceGraph);
        IGraph AddChoice(IGraph concatGraph);
        bool IsEmpty { get; }
        void RepeatN();
        void RepeatPlus();
    }

    public class Graph : IGraph
    {
        public Node StartNode { get; }

        public Graph()
            => StartNode = new Node();

        public void AddSequence(IGraph sequenceGraph)
        {
            throw new System.NotImplementedException();
        }

        public IGraph AddChoice(IGraph concatGraph)
        {
            throw new System.NotImplementedException();
        }

        public bool IsEmpty { get; }
        public void RepeatN()
        {
            throw new System.NotImplementedException();
        }

        public void RepeatPlus()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Node
    {
    }
}
