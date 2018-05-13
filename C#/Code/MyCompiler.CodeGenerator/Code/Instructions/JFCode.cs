using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class JFCode : CmsCode
    {
        public JFCode() : base(Instruction.JF, 0X5C)
        {
        }
    }
}
