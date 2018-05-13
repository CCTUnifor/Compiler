using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompiler.Core.Extensions
{
    public static class NumberExtension
    {
        public static string ToHexadecimal(this int value) => value.ToString("X4");
    }
}
