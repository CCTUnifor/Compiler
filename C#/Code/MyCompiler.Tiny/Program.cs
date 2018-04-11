using System;
using System.IO;
using MyCompiler.Core.Models.SyntacticAnalyzes;

namespace MyCompiler.TinyApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                var fileName = "my-programm.txt";
                var tiny = new TinySyntacticAnalyzer();
                var countLine = 0;

                Console.WriteLine("# Tiny Grammar");

                var code = "";
                var input = new StreamReader(fileName);
                code = input.ReadToEnd();

                Console.WriteLine($"++++++ Example file: {fileName} +++++++\n");
                Console.WriteLine(code);

                Console.WriteLine($"\n++++++ RESULT +++++++\n");
                tiny.Parser(++countLine, code);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n\n{e}");
            }

            Console.ReadLine();
        }
    }
}
