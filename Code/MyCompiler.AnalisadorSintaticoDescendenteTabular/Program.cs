using System;
using System.IO;
using ConsoleTable;
using MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA;

namespace MyCompiler.AnalisadorSintaticoDescendenteTabular
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                ConsoleTableOptions.Pad = 100;
                ConsoleTableOptions.DefaultIfNull = "Error";
                Printable.Printable.PathToSave = $"Logs/log{DateTime.Now.Millisecond}.txt";

                //Printable.Printable.PrintLn("ε");
                Printable.Printable.PrintLn("# Analisador Sintatico Descendente Tabular");

                const string grammarFile = "grammar(0).txt";
                const string inputFile = "input(0).txt";

                var grammar = Read($"grammars/{grammarFile}");
                var input = Read($"inputs/{inputFile}");

                PrintGrammar(grammar);
                PrintInput(input);

                var syntacticAnalysis = new NonRecursiveDescendingSyntacticAnalysis(grammar);
                syntacticAnalysis.Parser(input);
            }
            catch (Exception e)
            {
                Printable.Printable.PrintLn(e);
            }

            Console.ReadLine();
        }

        private static void PrintInput(string input)
        {
            Printable.Printable.PrintLn("\n++++++ Input ++++++\n");
            Printable.Printable.PrintLn(input);
            Printable.Printable.PrintLn("\n");
        }

        private static string Read(string path)
        {
            var input = new StreamReader($"{path}");
            var code = input.ReadToEnd();

            return code;
        }

        private static void PrintGrammar(string grammar)
        {
            Printable.Printable.PrintLn("\n++++++ Grammar ++++++\n");
            Printable.Printable.PrintLn(grammar);
        }
    }
}