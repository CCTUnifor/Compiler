using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class LODCode : CmsCodeReference
    {
        public LODCode(CmsCode reference) : base(Instruction.LOD, 0X40, reference)
        {
        }
    }
}
