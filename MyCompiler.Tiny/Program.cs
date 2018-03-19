using System;
using System.Collections.Generic;
using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Core.Interfaces;
using MyCompiler.Core.Models.GraphModels;

namespace MyCompiler.TinyApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var tiny = new TinySyntacticAnalyzer();
        }
    }

    public class TinySyntacticAnalyzer : ISyntacticAnalyzer<RegularExpressionGrammarClass>
    {
        public TinyLexicalAnalyze LexicalAnalyze { get; set; }

        public TinySyntacticAnalyzer()
        {
            LexicalAnalyze = new TinyLexicalAnalyze();
        }

        public IGraph Check(IEnumerable<IToken<RegularExpressionGrammarClass>> tokens)
        {
            throw new NotImplementedException();
        }
    }

    public class TinyLexicalAnalyze
    {
        public TinyToken GetNextToken()
        {
            return null;
        }
    }
    public class TinyToken
    {
    }


}
