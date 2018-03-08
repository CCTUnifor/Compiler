using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Core.Interfaces;
using MyCompiler.Core.Models;
using MyCompiler.Core.Models.LexicalAnalyzer;
using MyCompiler.Core.Models.SyntacticAnalyzes;

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

    public class RegularExpressionParser
    {
        private string _input { get; set; }

        public RegularExpressionParser(string input)
        {
            _input = input;
        }

        public char Peek()
            => _input[0];

        public void Eat(char c)
        {
            if (Peek() == c)
                _input = _input.Substring(1);
            else
                throw new Exception($"Expected: {c}; got: {Peek()}");
        }

        public char Next()
        {
            var c = Peek();
            Eat(c);
            return c;
        }

        public bool More()
            => _input.Any();
    }
}
