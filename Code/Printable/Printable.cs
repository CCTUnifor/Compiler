using System;
using System.IO;

namespace Printable
{
    public static class Printable
    {
        public static string PathToSave { get; set; }
        public static bool IsToPrintInConsole { get; set; } = true;

        public static void Print(object v)
        {
            if (IsToPrintInConsole)
                Console.Write(v);
            if (!string.IsNullOrEmpty(PathToSave))
                PrintInFile(v);
        }

        public static void PrintLn(object v)
        {
            if (IsToPrintInConsole)
                Console.WriteLine(v);
            if (!string.IsNullOrEmpty(PathToSave))
                PrintInFile(v, true);
        }

        private static void PrintInFile(object v, bool printLn = false)
        {
            if (string.IsNullOrEmpty(PathToSave))
                return;

            Check();
            using (var file = File.AppendText(PathToSave))
            {
                if (printLn)
                    file.WriteLine(v.ToString());
                else
                    file.Write(v.ToString());
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
