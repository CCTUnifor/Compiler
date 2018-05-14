using System;
using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;

namespace MyCompiler.CodeGenerator.StatmentHandlers
{
    public class WhileStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            var expressionStatmentHandler = new ExpressionStatmentHandler();
            expressionStatmentHandler.Handler(generator);

            var cmsCodeReference = (CmsCodeReference)CmsCodeFactory.JF(new CmsCode(generator.CodesLengh));
            generator.JFCode.Push(cmsCodeReference);
            generator.AddCode(cmsCodeReference);

            var commeBackWhile = (CmsCodeReference)CmsCodeFactory.JMP(expressionStatmentHandler.InitialCode);
            generator.InitialWhileCode.Push(commeBackWhile);

            generator.State = CmsCodeState.Initial;
        }
    }
}