using System;
using System.IO;
using CCTUnifor.ConsoleTable;
using CCTUnifor.Logger;
using MyCompiler.CodeGenerator;
using MyCompiler.Parser;

namespace MyCompiler.ConsoleApp.CodeGeneratorTM
{
    class Program
    {

        public static void ConfigConsole()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ConsoleTableOptions.Pad = 60;
            ConsoleTableOptions.DefaultIfNull = "Error";
            Logger.PathToSave = $"Logs/log{CmsCodeGenerator.Milliseconds}.txt";

            Logger.PrintHeader("# Code Generator - TM");
        }

        //[ConfigConsoleAspect]
        private static void Main(string[] args)
        {
            try
            {
                const string grammarFile = "grammar(0).txt";
                const string inputFile = "input(0).txt";

                var grammar = Read("Grammar", $"grammars/{grammarFile}");
                var input = Read("Input", $"inputs/{inputFile}");

                var parser = new TopDownParser(grammar);
                parser.Parser(input);

                var codeGenerator = new TmCodeGenerator(parser, input);
                codeGenerator.Generator();
                codeGenerator.Export();
                codeGenerator.ExecuteVM();
            }
            catch (Exception e)
            {
                Logger.PrintLn(e);
            }

            Console.ReadLine();
        }

        private static string Read(string type, string path)
        {

            var input = new StreamReader($"{path}");
            var code = input.ReadToEnd();

            Logger.PrintHeader(type);
            Logger.PrintLn(code);

            return code;
        }
    }
}
