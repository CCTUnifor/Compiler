using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class EQCode : CmsCode
    {
        public EQCode() : base(Instruction.EQ, 0X20, "X2")
        {
        }
    }
}
