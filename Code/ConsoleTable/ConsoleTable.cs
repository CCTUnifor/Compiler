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
        private int Pad = 20;

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
            for (int i = 0; i < Rows.Count; i++)
            {
                Console.Write($"|{RowsHeader[i].PadRight(Pad)} ");
                for (int j = 0; j < CollumnsHeader.Length - 1; j++)
                {
                    Console.Write($"| {Rows[i][j].PadRight(Pad)}");
                }
                Console.WriteLine("|");
            }

            //for (int i = 0; i < CollumnsHeader.Length; i++)
            //{
            //    Console.Write($"| {CollumnsHeader[i].PadRight(Pad)}");
            //}
            //Console.WriteLine("|");
        }

        private void WriteHeader()
        {
            for (int i = 0; i < CollumnsHeader.Length; i++)
            {
                Console.Write($"| {CollumnsHeader[i].PadRight(Pad)}");
            }
            Console.WriteLine("|");
            for (int i = 0; i < CollumnsHeader.Length; i++)
            {
                Console.Write($"|{"".PadRight(Pad + 1, '-')}");
            }
            Console.WriteLine("|");
        }
    }
}
