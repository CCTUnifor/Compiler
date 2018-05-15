using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCompiler.Core.Extensions
{
    public static class HexExtension
    {
        public static byte[] ToConvertByte(this string hex) => Enumerable.Range(0, hex.Length / 2).Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16)).ToArray();
    }
}
