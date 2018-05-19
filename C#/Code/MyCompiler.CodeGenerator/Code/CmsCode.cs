using System;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.Core.Extensions;

namespace MyCompiler.CodeGenerator.Code
{
    public class CmsCode
    {
        private readonly string _hexadecimalFormat;
        public Instruction Instruction { get; private set; }

        public int ValueDecimal { get; set; }
        public bool ReverseBytes { get; }
        public string Value => ValueDecimal.ToHexadecimal(_hexadecimalFormat);
        public virtual byte[] Bytes
        {
            get
            {
                var convertByte = Value.ToConvertByte();
                if (ReverseBytes)
                    Array.Reverse(convertByte);
                return convertByte;
            }
        }

        protected int PadRigth { get; set; } = 5;

        public CmsCode(int value, string hexadecimalFormat = "X4", bool reverseBytes = false)
        {
            ValueDecimal = value;
            Instruction = Instruction.Reference;
            _hexadecimalFormat = hexadecimalFormat;
        }

        public CmsCode(Instruction instruction, int valueDecimal, string hexadecimalFormat = "X4", bool reverseBytes = false)
        {
            ValueDecimal = valueDecimal;
            ReverseBytes = reverseBytes;
            Instruction = instruction;
            _hexadecimalFormat = hexadecimalFormat;
        }

        public override string ToString() => $"{BitConverter.ToString(Bytes).PadRight(PadRigth)} {"".PadRight(2, ' ')} - [{Instruction}]";
        public string Export() => Value;
        public virtual int Length => Bytes.Length;
    }
}