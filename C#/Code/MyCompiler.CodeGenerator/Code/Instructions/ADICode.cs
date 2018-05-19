using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class ADICode : CmsCodeReference
    {
        public ADICode(CmsCode reference) : base(Instruction.ADI, 0X14, reference, "X2")
        {
        }
    }
}
