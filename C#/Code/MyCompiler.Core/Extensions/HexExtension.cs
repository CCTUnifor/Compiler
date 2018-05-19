using System;
using System.Linq;

namespace MyCompiler.Core.Extensions
{
    public static class HexExtension
    {
        public static byte[] ToConvertByte(this string hex)
        {
            var convertByte = Enumerable.Range(0, hex.Length / 2).Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16))
                .ToArray();
            Array.Reverse(convertByte);
            return convertByte;
        }
    }
}
