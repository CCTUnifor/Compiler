using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;

namespace MyCompiler.CodeGenerator.StatmentHandlers.CMS
{
    public class UntilStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            var exp = new ExpressionStatmentHandler();
            exp.Handler(generator);

            var repeatStartReference = generator.RepeatReferenceStack.Pop();
            var jf = (CmsCodeReference)CmsCodeFactory.JF(new CmsCode(generator.CodesLengh));
            var jmp = CmsCodeFactory.JMP(repeatStartReference);
            generator.AddCode(jf);
            generator.AddCode(jmp);

            jf.Reference.ValueDecimal = generator.CodesLengh;
            generator.GeneratorState = TinyCodeGeneratorState.Initial;
        }
    }
}