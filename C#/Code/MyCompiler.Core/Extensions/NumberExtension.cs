using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompiler.Core.Extensions
{
    public static class NumberExtension
    {
        public static string ToHexadecimal(this int value, string hexadecimalFormat = "X4") => value.ToString(hexadecimalFormat);
    }
}
