using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class MUICode : CmsCode
    {
        public MUICode() : base(Instruction.MUI, 0X16)
        {
        }
    }
}
