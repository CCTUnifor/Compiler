using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class SUICode : CmsCode
    {
        public SUICode() : base(Instruction.SUI, 0X15)
        {
        }
    }
}
