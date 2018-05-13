using MyCompiler.CodeGenerator.Code.Instructions;
using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code
{
    public class CmsCodeReference : CmsCode
    {
        public readonly CmsCode Reference;

        public CmsCodeReference(Instruction instruction, int valueDecimal, CmsCode reference, string hexadecimalFormat = "X2") : base(instruction, valueDecimal, hexadecimalFormat)
        {
            Reference = reference;
        }

        public override string ToString() 
            => $"{Value.PadRight(PadRigth)} {Reference.Value.PadRight(PadRigth, ' ')} - [{Instruction}]";

        public override int Length => 3;
    }
}