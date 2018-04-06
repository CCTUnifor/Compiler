using System;

namespace MyCompiler.Core.Exceptions
{
    public class CompilationException : Exception
    {
        public CompilationException(string s) : base($"\n** Compilation exception: **\n{s}\n\n")
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
    }
}