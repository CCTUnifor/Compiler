using System.Collections.Generic;
using System.Linq;
using CCTUnifor.ConsoleTable;
using CCTUnifor.Logger;

namespace MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA
{
    public class PrintInfo
    {
        public static void PrintFirsts(IEnumerable<First> Firsts)
        {
            Logger.PrintHeader("Firsts");
            foreach (var first in Firsts)
                Logger.PrintLn(first.ToString());
            Logger.PrintLn("\n");
        }

        public static void PrintFollows(IEnumerable<Follow> Follows)
        {
            Logger.PrintHeader("Follows");
            foreach (var follow in Follows)
                Logger.PrintLn(follow.ToString());
            Logger.PrintLn("\n");
        }

        public static void PrintTable(ICollection<Terminal> Terminals, ICollection<NonTerminal> NonTerminals, Term[,] Table)
        {
            Logger.PrintHeader("Table");

            var collumnsHeader = Terminals.Select(x => x.Value).ToArray();
            var rowsHeader = NonTerminals.Select(x => x.Value.ToString()).ToArray();

            var tab = new ConsoleTable(collumnsHeader, rowsHeader);
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

            Logger.PrintLn("\n");
        }
    }
}