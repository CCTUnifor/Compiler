using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class STOCode : CmsCodeReference
    {
        public STOCode(CmsCode reference) : base(Instruction.STO, 0X41, reference)
        {
        }
    }
}
