using System;
using System.IO;

namespace Printable
{
    public static class Printable
    {
        public static string PathToSave { get; set; }
        public static bool IsToPrintInConsole { get; set; } = true;
        public static ConsoleColor ConsoleColorHeader { get; set; } = ConsoleColor.Yellow;

        public static void Print(object v)
        {
            if (IsToPrintInConsole)
                Console.Write(v);
            if (!string.IsNullOrEmpty(PathToSave))
                PrintInFile(v);
        }

        public static void PrintLn(object value, ConsoleColor? v = null)
        {
            if (v.HasValue)
                Console.ForegroundColor = v.Value;

            if (IsToPrintInConsole)
                Console.WriteLine(value);
            if (!string.IsNullOrEmpty(PathToSave))
                PrintInFile(value, true);

            if (v.HasValue)
                Console.ResetColor();
        }

        public static void PrintHeader(string input)
        {
            Console.ForegroundColor = ConsoleColorHeader;
            var notation = "#".PadRight(30, '#');
            PrintLn($"\n{notation} {input} {notation}\n");
            Console.ResetColor();
        }

        public static void PrintLnSuccess(object v)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            PrintLn($"\n{v}");
            Console.ResetColor();
        }

        private static void PrintInFile(object v, bool printLn = false)
        {
            if (string.IsNullOrEmpty(PathToSave))
                return;

            Check();
            using (var file = File.AppendText(PathToSave))
            {
                if (printLn)
                    file.WriteLine(v);
                else
                    file.Write(v);
            }

        }

        public static void ClearFile()
        {
            Check();
            File.WriteAllText(PathToSave, string.Empty);
        }

        private static void Check()
        {
            if (!Directory.Exists(Path.GetDirectoryName(PathToSave)))
                Directory.CreateDirectory(Path.GetDirectoryName(PathToSave));
        }
    }
}
