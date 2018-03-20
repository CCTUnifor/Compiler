using System;
using MyCompiler.Core.Models.LexicalAnalyzes;

namespace MyCompiler.Core.Models.SyntacticAnalyzes
{
    public class TinySyntacticAnalyzer
    {
        public TinyLexicalAnalyze LexicalAnalyze { get; set; }

        public void Check(int countLine, string input)
        {
            LexicalAnalyze = new TinyLexicalAnalyze(countLine, input);

            var firsttoken = LexicalAnalyze.GetNextToken();
            var second = LexicalAnalyze.GetNextToken();
            var d = LexicalAnalyze.GetNextToken();
            var d2 = LexicalAnalyze.GetNextToken();
            //throw new NotImplementedException();
        }
    }
}