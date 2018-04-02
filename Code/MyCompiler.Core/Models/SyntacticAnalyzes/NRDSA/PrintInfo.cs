using System.Collections.Generic;
using System.Linq;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class PrintInfo
    {
        public static void PrintFirsts(IEnumerable<First> Firsts)
        {
            Printable.Printable.PrintLn("++++++ Firsts ++++++\n");
            foreach (var first in Firsts)
                Printable.Printable.PrintLn(first);
            Printable.Printable.PrintLn("\n");
        }

        public static void PrintFollows(IEnumerable<Follow> Follows)
        {
            Printable.Printable.PrintLn("++++++ Follows ++++++\n");
            foreach (var follow in Follows)
                Printable.Printable.PrintLn(follow);
            Printable.Printable.PrintLn("\n");
        }

        public static void PrintTable(ICollection<Terminal> Terminals, ICollection<NonTerminal> NonTerminals, Term[,] Table)
        {
            Printable.Printable.PrintLn("++++++ Table ++++++\n");

            var collumnsHeader = Terminals.Select(x => x.Value).ToArray();
            var rowsHeader = NonTerminals.Select(x => x.Value.ToString()).ToArray();

            var tab = new ConsoleTable.ConsoleTable(collumnsHeader, rowsHeader);
            for (var i = 0; i < NonTerminals.Count; i++)
            {
                var zxc = new List<Term>();
                for (var j = 0; j < Terminals.Count; j++)
                {
                    zxc.Add(Table[i, j]);
                }
                tab.AddRow(zxc.Select(x => x?.ToString() ?? "").ToArray());
            }

            tab.Write();

            Printable.Printable.PrintLn("\n");
        }
    }
}