using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class LTCode : CmsCode
    {
        public LTCode() : base(Instruction.LT, 0X24)
        {
        }
    }
}
