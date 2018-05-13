using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class STOCode : CmsCode
    {
        public STOCode() : base(Instruction.STO, 0X41)
        {
        }
    }
}
