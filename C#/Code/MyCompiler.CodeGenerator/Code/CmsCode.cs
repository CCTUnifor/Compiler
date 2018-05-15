using MyCompiler.CodeGenerator.Enums;
using MyCompiler.Core.Extensions;

namespace MyCompiler.CodeGenerator.Code
{
    public class CmsCode
    {
        private readonly string _hexadecimalFormat;
        public Instruction Instruction { get; set; }

        public int ValueDecimal { get; set; }
        public string Value => ValueDecimal.ToHexadecimal(_hexadecimalFormat);
        public virtual byte[] Bytes => Value.ToConvertByte();
        protected int PadRigth { get; set; } = 5;

        public CmsCode(int value, string hexadecimalFormat = "X4")
        {
            ValueDecimal = value;
            Instruction = Instruction.Reference;
            _hexadecimalFormat = hexadecimalFormat;
        }

        public CmsCode(Instruction instruction, int valueDecimal, string hexadecimalFormat = "X4")
        {
            ValueDecimal = valueDecimal;
            Instruction = instruction;
            _hexadecimalFormat = hexadecimalFormat;
        }

        public override string ToString() => $"{Value.PadRight(PadRigth)} {"".PadRight(PadRigth, ' ')} - [{Instruction}]";
        public string Export() => Value;
        public virtual int Length => 2;
    }
}