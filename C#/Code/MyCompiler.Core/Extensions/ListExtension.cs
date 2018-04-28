using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompiler.Core.Extensions
{
    public static class ListExtension
    {
        public static string ToConvertString(this IEnumerable<Object> v) => $"[ {string.Join(",", v)} ]";
    }
}
