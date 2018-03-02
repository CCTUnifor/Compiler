using System;
using MyCompiler.Core.Enums.MathExpression;
using MyCompiler.Core.Interfaces;
using MyCompiler.Core.Models;
using MyCompiler.Core.Models.LexicalAnalyzer;

namespace MyCompiler
{
    public class Program
    {
        private static void Main(string[] args)
        {
            ILexicalAnalyzer<MathExpressionGrammarClass> lexicalAnalyzer = new MathExpressionLexicalAnalyzer();

            try
            {
                Console.WriteLine("Write your Input: ");
                var input = Console.ReadLine();
                var tokens = lexicalAnalyzer.LoadTokens(input);

                Console.WriteLine("\n-----------------------------------------------------\n");
                Console.WriteLine("Result: ");
                foreach (var token in tokens)
                    Console.WriteLine($"{token.Value.PadRight(10)} - {token.GrammarClass}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }
    }
}
