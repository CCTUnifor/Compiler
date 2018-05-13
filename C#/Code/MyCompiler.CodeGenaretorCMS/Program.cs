using System;
using System.IO;
using CCTUnifor.Logger;
using MyCompiler.CodeGenerator;
using MyCompiler.Core.Aspects;
using MyCompiler.Parser;

namespace MyCompiler.CodeGenaretorCMS
{
    public class Program
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

                var parser = new TopDownParser(grammar);
                parser.Parser(input);

                var codeGenerator = new CmsCodeGenerator(parser, input);
                codeGenerator.Generator();

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
