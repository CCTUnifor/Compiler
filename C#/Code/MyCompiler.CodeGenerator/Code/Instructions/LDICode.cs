using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class LDICode : CmsCode
    {
        public LDICode() : base(Instruction.LDI, 0X44)
        {
        }
    }
}
