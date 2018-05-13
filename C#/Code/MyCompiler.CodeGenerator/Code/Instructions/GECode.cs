using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class GECode : CmsCode
    {
        public GECode() : base(Instruction.GE, 0X23)
        {
        }
    }
}
