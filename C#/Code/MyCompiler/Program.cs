using System;
using MyCompiler.Core.Exceptions;
using MyCompiler.Core.Models.ConstructionSubsets;
using MyCompiler.Core.Models.LexicalAnalyzer;
using MyCompiler.Core.Models.SyntacticAnalyzes;

namespace MyCompiler.RegularExpressionApp
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                Console.WriteLine("Write your Input: ");
                var input = Console.ReadLine();

                var lexicalAnalyzer = new RegularExpressionLexicalAnalyzer();
                var sintaxAnalyzer = new RegularExpressionSyntacticAnalyzer();
                var constructionSubsets = new ConstructionSubsets();

                var tokens = lexicalAnalyzer.LoadTokens(input);
                var thompsonConstruction = sintaxAnalyzer.Check(tokens);
                var locks = constructionSubsets.Generate(thompsonConstruction);
                constructionSubsets.PrintMatriz(locks);
                // a(a|b)*
            }
            catch (CompilationException)
            {
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
