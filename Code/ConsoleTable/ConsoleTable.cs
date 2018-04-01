using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleTable
{
    public class ConsoleTable
    {
        public string[] CollumnsHeader { get; private set; }
        public string[] RowsHeader { get; private set; }
        public List<List<string>> Rows { get; private set; }

        public ConsoleTable(string[] collumnsHeader, string[] rowsHeader)
        {
            var c = new[] { "" }.ToList();
            c.AddRange(collumnsHeader.ToList());

            CollumnsHeader = c.ToArray();
            RowsHeader = rowsHeader;
            Rows = new List<List<string>>();
        }

        public void AddRow(string[] row)
        {

            Rows.Add(row.ToList());
        }

        public void Write()
        {
            WriteHeader();
            WriteRows();
        }

        private void WriteRows()
        {
            for (var i = 0; i < Rows.Count; i++)
            {
                Console.Write($"|{RowsHeader[i].PadRight(3)} ");
                for (var j = 0; j < CollumnsHeader.Length - 1; j++)
                {
                    var value = string.IsNullOrEmpty(Rows[i][j]) ? ConsoleTableOptions.DefaultIfNull : Rows[i][j];
                    Console.Write($"| {value.PadRight(ConsoleTableOptions.Pad)}");
                }
                Console.WriteLine("|");
            }
        }

        private void WriteHeader()
        {
            Console.Write($"| {CollumnsHeader[0].PadRight(3)}");

            for (var i = 1; i < CollumnsHeader.Length; i++)
                Console.Write($"| {CollumnsHeader[i].PadRight(ConsoleTableOptions.Pad)}");

            Console.WriteLine("|");

            Console.Write($"|{"".PadRight(3 + 1, '-')}");
            for (var i = 1; i < CollumnsHeader.Length; i++)
                Console.Write($"|{"".PadRight(ConsoleTableOptions.Pad + 1, '-')}");

            Console.WriteLine("|");
        }
    }

    public static class ConsoleTableOptions
    {
        public static int Pad { get; set; } = 15;
        public static string DefaultIfNull { get; set; } = "";
    }
}
