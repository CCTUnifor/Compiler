using System;
using System.Diagnostics;
using System.IO;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using CCTUnifor.ConsoleTable;
using CCTUnifor.Logger;
using MyCompiler.CodeGenerator;
using MyCompiler.Core.Aspects;
using MyCompiler.Core.Interfaces.Parsers;
using MyCompiler.Parser;

namespace MyCompiler.CodeGenaretorCMS
{
    public class Program
    {

        public static void ConfigConsole()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ConsoleTableOptions.Pad = 60;
            ConsoleTableOptions.DefaultIfNull = "Error";
            Logger.PathToSave = $"Logs/log{CmsCodeGenerator.Milliseconds}.txt";

            Logger.PrintHeader("# Code Generator - CMS");
        }

        [ConfigConsoleAspect]
        private static void Main(string[] args)
        {
            ConfigConsole();
            try
            {

                var generator = new ProxyGeneratorBuilder()
                    .Configure(config =>
                    {
                        config.EnableParameterAspect();
                    })
                    .Build();
                //var service = generator.CreateInterfaceProxy<IParser, TopDownParser>();

                const string grammarFile = "grammar(0).txt";
                const string inputFile = "input(0).txt";

                var grammar = Read("Grammar", $"grammars/{grammarFile}");
                var input = Read("Input", $"inputs/{inputFile}");

                var parser = new TopDownParser(grammar);
                parser.Parser(input);

                var codeGenerator = new CmsCodeGenerator(parser, input);
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

        

        [LogReadFileAspect]
        private static string Read(string type, string path)
        {
            var input = new StreamReader($"{path}");
            var code = input.ReadToEnd();

            return code;
        }
    }
}
