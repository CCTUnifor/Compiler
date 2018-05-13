using MyCompiler.CodeGenerator.Enums;
using MyCompiler.Core.Extensions;

namespace MyCompiler.CodeGenerator.Code
{
    public class CmsCode
    {
        public Instruction Instruction { get; set; }

        public int ValueDecimal { get; set; }
        public string Value { get; set; }

        public CmsCode(int value)
        {
            ValueDecimal = value;
            Value = value.ToHexadecimal();
        }

        public CmsCode(Instruction instruction, int valueDecimal)
        {
            ValueDecimal = valueDecimal;
            Instruction = instruction;
            Value = ValueDecimal.ToHexadecimal();
        }

        public override string ToString() => $"{Value} - [{Instruction}]";
    }
}