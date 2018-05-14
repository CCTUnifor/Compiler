using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class NECode : CmsCode
    {
        public NECode() : base(Instruction.NE, 0X21, "X2")
        {
        }
    }
}
