using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class STOPCode : CmsCode
    {
        public STOPCode() : base(Instruction.STOP, 0X61, "X2")
        {
        }
    }
}
