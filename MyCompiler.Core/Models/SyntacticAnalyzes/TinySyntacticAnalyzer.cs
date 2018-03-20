using System;
using MyCompiler.Core.Enums;
using MyCompiler.Core.Exceptions;
using MyCompiler.Core.Models.LexicalAnalyzes;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Core.Models.SyntacticAnalyzes
{
    public class TinySyntacticAnalyzer
    {
        public TinyLexicalAnalyze LexicalAnalyze { get; set; }

        public TinyToken Peek => LexicalAnalyze.LastToken;
        private bool HasNex() => LexicalAnalyze.Any();

        private void Eat(char c)
        {
            if (Peek == null)
                throw new ExpectedException("c", "null");
            if (c.ToString().ToLower() != Peek.Value.ToLower()) // TODO
                throw new ExpectedException(c.ToString(), Peek.Value);

            Console.WriteLine($"++++++ EAT - {c}++++++");
            LexicalAnalyze.GetNextToken();
        }

        public void Check(int countLine, string input)
        {
            LexicalAnalyze = new TinyLexicalAnalyze(countLine, input);

            DeclSequencia();
            if (!HasNex() || Peek.Grammar != TinyGrammar.SemiColon) return;

            Eat(';');
            DeclSequencia();
        }

        private void DeclSequencia()
        {
            throw new NotImplementedException();
        }
    }
}