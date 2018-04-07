using System;
using System.IO;
using CCTUnifor.ConsoleTable;
using CCTUnifor.Logger;
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
                ConsoleTableOptions.Pad = 60;
                ConsoleTableOptions.DefaultIfNull = "Error";
                Logger.PathToSave = $"Logs/log{DateTime.Now.Millisecond}.txt";

                Logger.PrintHeader("# Analisador Sintatico Descendente Tabular");

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
                Logger.PrintLn(e);
            }

            Console.ReadLine();
        }

        private static void PrintInput(string input)
        {
            Logger.PrintHeader("Input");
            Logger.PrintLn(input);
            Logger.PrintLn("\n");
        }

        private static string Read(string path)
        {
            var input = new StreamReader($"{path}");
            var code = input.ReadToEnd();

            return code;
        }

        private static void PrintGrammar(string grammar)
        {
            Logger.PrintHeader("Grammar");
            Logger.PrintLn(grammar);
        }
    }
}