using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Instructions
{
    public class LODCode : CmsCode
    {
        public LODCode() : base(Instruction.LOD, 0X40)
        {
        }
    }
}
