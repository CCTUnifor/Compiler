using System;
using MyCompiler.Core.Interfaces;
using MyCompiler.Core.Models;

namespace MyCompiler
{
    public class Program
    {
        private static void Main(string[] args)
        {
            //ILexicalAnalyzer lexicalAnalyzer = new MathExpressionLexicalAnalyzer();
            ILexicalAnalyzer lexicalAnalyzer = new GenericLexicalAnalyzer(new string[0]);

            try
            {
                Console.WriteLine("Write your Input: ");
                var input = Console.ReadLine();
                var tokens = lexicalAnalyzer.LoadTokens(input);

                Console.WriteLine("\n-----------------------------------------------------\n");
                Console.WriteLine("Result: ");
                //foreach (var token in tokens)
                //    Console.WriteLine($"{token.Value.PadRight(10)} - {token.GrammarClasse}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }
    }
}
