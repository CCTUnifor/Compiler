using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class LECode : CmsCode
    {
        public LECode() : base(Instruction.LE, 0X25, "X2")
        {
        }
    }
}
