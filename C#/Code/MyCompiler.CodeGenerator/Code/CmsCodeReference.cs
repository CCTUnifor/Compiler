using System;
using System.Linq;
using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code
{
    public class CmsCodeReference : CmsCode
    {
        public readonly CmsCode Reference;

        public CmsCodeReference(Instruction instruction, int valueDecimal, CmsCode reference,
            string hexadecimalFormat = "X2", bool reverseBytes = false)
            : base(instruction, valueDecimal, hexadecimalFormat, reverseBytes)
        {
            Reference = reference;
        }

        public override string ToString()
            => $"{BitConverter.ToString(Bytes).PadRight(PadRigth)} - [{Instruction}]";

        public override int Length => Bytes.Length;
        public override byte[] Bytes
        {
            get
            {
                var referenceBytes = Reference.Bytes.ToArray();
                if (ReverseBytes)
                    Array.Reverse(referenceBytes);
                return base.Bytes.Concat(referenceBytes).ToArray();
            }
        }
    }
}