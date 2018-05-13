using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class INCode : CmsCode
    {
        public INCode() : base(Instruction.IN, 0X57)
        {
        }
    }
}
