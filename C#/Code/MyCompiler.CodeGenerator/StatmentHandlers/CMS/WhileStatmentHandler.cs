using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;

namespace MyCompiler.CodeGenerator.StatmentHandlers.CMS
{
    public class WhileStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            var startWhileReference = new CmsCode(generator.CodesLengh);

            var expressionStatmentHandler = new ExpressionStatmentHandler();
            expressionStatmentHandler.Handler(generator);

            var cmsCodeReference = (CmsCodeReference)CmsCodeFactory.JF(new CmsCode(0X00));
            generator.JFCodeReferenceStack.Push(cmsCodeReference);
            generator.AddCode(cmsCodeReference);

            var commeBackWhile = (CmsCodeReference)CmsCodeFactory.JMP(startWhileReference);
            generator.StartWhileCodeReference.Push(commeBackWhile);

            generator.GeneratorState = TinyCodeGeneratorState.Initial;
        }
    }
}