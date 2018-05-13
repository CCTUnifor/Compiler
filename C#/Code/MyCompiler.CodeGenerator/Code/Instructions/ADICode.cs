using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class ADICode : CmsCode
    {
        public ADICode() : base(Instruction.ADI, 0X14)
        {
        }
    }
}
