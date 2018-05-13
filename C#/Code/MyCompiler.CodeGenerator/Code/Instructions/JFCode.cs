using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class JFCode : CmsCodeReference
    {
        public JFCode(CmsCode reference) : base(Instruction.JF, 0X5C, reference)
        {
        }
    }
}
