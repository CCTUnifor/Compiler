using System;
using System.IO;
using CCTUnifor.Logger;
using MyCompiler.Tokenization;
using MyCompiler.TopDownApp.Aspects;

namespace MyCompiler.TopDownApp
{
    public static class Program
    {
        [ConfigConsoleAspect]
        private static void Main(string[] args)
        {
            try
            {
                const string grammarFile = "grammar(0).txt";
                const string inputFile = "input(0).txt";

                var grammar = Read("Grammar", $"grammars/{grammarFile}");
                var input = Read("Input", $"inputs/{inputFile}");

                var syntacticAnalysis = new TopDownTokenization(grammar);
                syntacticAnalysis.Parser(input);
            }
            catch (Exception e)
            {
                Logger.PrintLn(e);
            }

            Console.ReadLine();
        }

        [LogReadFileAspect]
        private static string Read(string type, string path)
        {
            var input = new StreamReader($"{path}");
            var code = input.ReadToEnd();

            return code;
        }
    }
}