using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class GTCode : CmsCode
    {
        public GTCode() : base(Instruction.GT, 0X22, "X2")
        {
        }
    }
}
