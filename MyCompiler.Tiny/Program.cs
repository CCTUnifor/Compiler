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
                Console.WriteLine("Write your Input: ");
                var input = new StreamReader("my-programm.txt");
                var tiny = new TinySyntacticAnalyzer();
                var countLine = 0;

                //while ((line = ) != null)
                tiny.Check(++countLine, input.ReadToEnd());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }

            Console.ReadLine();
        }
    }
}
