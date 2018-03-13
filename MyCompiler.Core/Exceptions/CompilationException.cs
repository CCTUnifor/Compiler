using System;

namespace MyCompiler.Core.Exceptions
{
    public class CompilationException : Exception
    {
        public CompilationException(string s) : base(s)
        {
            Console.WriteLine($"** Compilation exception: {s}");
        }
    }
}