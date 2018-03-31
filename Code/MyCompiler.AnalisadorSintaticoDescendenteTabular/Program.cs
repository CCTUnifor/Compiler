using System;
using MyCompiler.Core.Models.SyntacticAnalyzes;

namespace MyCompiler.AnalisadorSintaticoDescendenteTabular
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                //Console.WriteLine("ε");
                Console.WriteLine("# Analisador Sintatico Descendente Tabular");
                var grammar = $"D -> TX\n" +
                              $"X -> +TX | -TX | ε\n" +
                              $"T -> FY\n" +
                              $"Y -> *FY | %FY | ε\n" +
                              $"F -> (D) | ide | num";
                //var grammar = $"S -> XYZ\n" +
                //              $"X -> aXb | ε\n" +
                //              $"Y -> cYZcX | d\n" +
                //              $"Z -> eZYe | f";
                //var grammar = "E -> Ba\n" +
                //              "B -> b | ε";
                //var grammar = "E -> ABC\n" +
                //              "A -> a | ε\n" +
                //              "B -> b | ε\n" +
                //              "C -> c | ε";
                //var grammar = "S -> AB\n" +
                //              "A -> c | ε\n" +
                //              "B -> cbB | ca";
                PrintGrammar(grammar);

                var syntacticAnalysis = new NonRecursiveDescendingSyntacticAnalysis(grammar);
                syntacticAnalysis.Parser();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }

        private static void PrintGrammar(string grammar)
        {
            Console.WriteLine("\n++++++ Grammar ++++++\n");
            Console.WriteLine(grammar);
            Console.WriteLine("\n");
        }
    }
}
