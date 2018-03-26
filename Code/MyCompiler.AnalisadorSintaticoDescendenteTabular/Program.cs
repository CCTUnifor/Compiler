using System;
using System.Linq;

namespace MyCompiler.AnalisadorSintaticoDescendenteTabular
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("# Analisador Sintatico Descendente Tabular");
                var grammar = $"S -> XYZ\nX -> aXb | E\nY -> cYZcX | d\nZ -> eZYe | f";

                Console.WriteLine("\n++++++ Grammar ++++++\n");
                Console.WriteLine(grammar);
                Console.WriteLine("\n");

                First(grammar);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }

        private static void First(string grammar)
        {
            var lines = grammar.Split("\n");
            foreach (var line in lines)
            {
                var caller = line.Split("->").FirstOrDefault().TrimEnd();
                var called = line.Split("->").LastOrDefault().TrimStart();
                Console.WriteLine($"{caller} -> {called}");

                foreach (var X1 in called)
                {
                    if (IsTerminal(X1))
                    {
                        // First(Caller) = { X1 };
                    }
                    else
                    {

                    }
                }
            }
        }

        private static bool IsTerminal(char c) => char.IsLower(c);
    }
}
