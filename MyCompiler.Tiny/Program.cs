using System;
using System.IO;
using MyCompiler.Core.Models.SyntacticAnalyzes;

namespace MyCompiler.TinyApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var fileName = "my-programm.txt";
                var input = new StreamReader(fileName);
                var tiny = new TinySyntacticAnalyzer();
                var countLine = 0;

                var example = input.ReadToEnd();
                Console.WriteLine($"++++++ Example file: {fileName} +++++++\n");
                Console.WriteLine(example);
                Console.WriteLine("\n++++++ RESULT ++++++ \n");

                tiny.Check(++countLine, example);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }
    }
}
