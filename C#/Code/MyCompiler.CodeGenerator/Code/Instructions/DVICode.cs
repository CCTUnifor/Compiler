using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class DVICode : CmsCode
    {
        public DVICode() : base(Instruction.DVI, 0X17)
        {
        }
    }
}
