using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class LDICode : CmsCodeReference
    {
        public LDICode(CmsCode reference) : base(Instruction.LDI, 0X44, reference, "X2")
        {
        }
    }
}
