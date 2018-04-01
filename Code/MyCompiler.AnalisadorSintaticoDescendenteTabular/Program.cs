using System;
using ConsoleTable;
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
                ConsoleTableOptions.Pad = 30;
                ConsoleTableOptions.DefaultIfNull = "Error";
                Printable.Printable.PathToSave = $"Logs/log{DateTime.Now.Millisecond}.txt";

                //Printable.Printable.PrintLn("ε");
                Printable.Printable.PrintLn("# Analisador Sintatico Descendente Tabular");

                var grammar = SetGrammar();
                var input = "( ide + ide ) $";
                //var input = "x * ( ide + ide ) $";

                PrintGrammar(grammar);

                var syntacticAnalysis = new NonRecursiveDescendingSyntacticAnalysis(grammar);
                syntacticAnalysis.Parser(input);
            }
            catch (Exception e)
            {
                Printable.Printable.PrintLn(e);
            }

            Console.ReadLine();
        }

        private static string SetGrammar()
        {
            return "E -> T X\n" +
                   "X -> + T X | - T X | ε\n" +
                   "T -> F Y\n" +
                   "Y -> * F Y | % F Y | ε\n" +
                   "F -> ( E ) | ide | num";
            //return "E -> TX\n" +
            //              "X -> +TX | ε\n" +
            //              "T -> FY\n" +
            //              "Y -> *FY | ε\n" +
            //              "F -> (E) | ide | num";

            //var grammar = "E -> TE'\n" +
            //              "E' -> +TE' | ε\n" +
            //              "T -> FY\n" +
            //              "Y -> *FY | ε\n" +
            //              "F -> (E) | ide | num";
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
            //var grammar = "E -> TX\n" +
            //              "X -> +TX | ε\n" +
            //              "T -> FY\n" +
            //              "Y -> *FY | ε\n" +
            //              "F -> (E) | id";
            //var grammar = "E -> TX\n" +
            //              "X -> vTX | ε\n" +
            //              "T -> FY\n" +
            //              "Y -> &FY | ε\n" +
            //              "F -> ¬F | id";

            //return "A -> B                           \n" +
            //       "B -> CD                                            \n" +
            //       "D ->  ; CD | ε                                     \n" +
            //       "C -> E | F | G | H | I                              \n" +
            //       "E ->  if J then B end | if J then B else B end      \n" +
            //       "F -> repeat B until J                               \n" +
            //       "G -> identificador := J                             \n" +
            //       "H -> read identificador                             \n" +
            //       "I -> write J                                        \n" +
            //       "J -> MLM | M                                      \n" +
            //       "L ->  < | =                                         \n" +
            //       "M -> PN                                            \n" +
            //       "N -> OPN | ε                                      \n" +
            //       "O ->  + | -                                         \n" +
            //       "P -> SQ                                            \n" +
            //       "Q -> RSQ | ε                                      \n" +
            //       "R -> * | /                                         \n" +
            //       "S -> ( J ) | numero | identificador                  ";
        }

        private static void PrintGrammar(string grammar)
        {
            Printable.Printable.PrintLn("\n++++++ Grammar ++++++\n");
            Printable.Printable.PrintLn(grammar);
            Printable.Printable.PrintLn("\n");
        }
    }
}