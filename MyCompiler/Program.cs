using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Core.Interfaces;
using MyCompiler.Core.Models.LexicalAnalyzer;

namespace MyCompiler.ConsoleApp
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Write your Input: ");
                var input = Console.ReadLine();

                //ILexicalAnalyzer<MathExpressionGrammarClass> lexicalAnalyzer = new MathExpressionLexicalAnalyzer();
                var lexicalAnalyzer = new RegularExpressionLexicalAnalyzer();
                var sintaxAnalyzer = new RegularExpressionSyntacticAnalyzer();

                var tokens = lexicalAnalyzer.LoadTokens(input).ToList();
                sintaxAnalyzer.Check(tokens);


                PrintTokens(tokens);
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void PrintTokens(IEnumerable<IToken<RegularExpressionGrammarClass>> tokens)
        {
            Console.WriteLine("\n-----------------------------------------------------\n");
            Console.WriteLine("Result: ");
            foreach (var token in tokens)
                Console.WriteLine($"{token.Value.PadRight(10)} - {token.GrammarClass}");
        }
    }

    public interface ISyntacticAnalyzer<T>
    {
        void Check(IEnumerable<IToken<T>> tokens);
    }

    public class RegularExpressionSyntacticAnalyzer : ISyntacticAnalyzer<RegularExpressionGrammarClass>
    {
        public void Check(IEnumerable<IToken<RegularExpressionGrammarClass>> tokens)
        {
            RE();
        }

        private void RE()
        {
            var isUnion = true;
            if (isUnion)
                UNION();
            else
                SimpleRE();
        }

        private void SimpleRE()
        {
            var isConcatenation = true;
            if (isConcatenation)
                Concatenation();
            else
                BasicRE();
        }

        private void BasicRE()
        {
            if (true)
                Star();
            else if (false)
                Plus();
            else if (false)
                ElementaryRE();
        }

        private void Star()
        {
            ElementaryRE();
            // if token.Value == *
        }

        private void Plus()
        {
            ElementaryRE();
            // if token.Value == +
        }

        private void ElementaryRE()
        {
            if (true)
                Group();
        }

        private void Group()
        {
            // if token.Value == (
            RE();
            // if token.Value == )
        }


        private void Concatenation()
        {
            SimpleRE();
            BasicRE();
        }

        private void UNION()
        {
            RE();
            // if token.Value == |
            SimpleRE();
        }
    }
}
