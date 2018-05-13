using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class OUTCode : CmsCode
    {
        public OUTCode() : base(Instruction.OUT, 0X58)
        {
        }
    }
}
