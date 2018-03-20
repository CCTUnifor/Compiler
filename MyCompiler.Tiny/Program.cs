using System;
using System.IO;
using System.Linq.Expressions;
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
                var line = "";

                while ((line = input.ReadLine()) != null)
                    tiny.Check(++countLine, line);
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
