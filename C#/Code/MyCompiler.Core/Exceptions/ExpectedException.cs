using System;

namespace MyCompiler.Core.Exceptions
{
    public class ExpectedException : Exception
    {
        public ExpectedException(string expected, string got, int? line)
        {
            var lineStr = line.HasValue ? $"Line: {line}" : "";
            Console.WriteLine($"\n*** Compilation Error:\n Expected: '{expected}' got: '{got}' {lineStr}");
        }
    }
}