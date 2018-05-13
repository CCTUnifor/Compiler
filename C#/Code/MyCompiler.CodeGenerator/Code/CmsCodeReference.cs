using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code
{
    public class CmsCodeReference : CmsCode
    {
        protected readonly CmsCode Reference;

        public CmsCodeReference(Instruction instruction, int valueDecimal, CmsCode reference) : base(instruction, valueDecimal)
        {
            Reference = reference;
        }

        public override string ToString() 
            => $"{Value} {Reference.Value} - [{Instruction}]";
    }
}