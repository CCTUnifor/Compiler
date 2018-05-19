using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class JMPCode : CmsCodeReference
    {
        public JMPCode(CmsCode reference) : base(Instruction.JMP, 0X5A, reference)
        {
        }
    }
}
