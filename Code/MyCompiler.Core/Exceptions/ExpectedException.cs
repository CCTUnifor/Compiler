using System;

namespace MyCompiler.Core.Exceptions
{
    public class ExpectedException : Exception
    {
        public ExpectedException(string expected, string got) : base($"Expected: {expected} got: {got}")
        {
        }
    }
}