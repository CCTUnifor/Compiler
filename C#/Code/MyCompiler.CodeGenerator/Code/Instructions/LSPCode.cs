using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class LSPCode : CmsCodeReference
    {
        public LSPCode(CmsCode reference) : base(Instruction.LSP, 0X4F, reference, "X2", true)
        {
        }
    }
}
