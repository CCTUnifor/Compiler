using System;
using System.Collections.Generic;
using System.Text;
using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar;

namespace MyCompiler.CodeGenerator.StatmentHandlers
{
    public class IfStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            var expressionStatmentHandler = new ExpressionStatmentHandler();
            expressionStatmentHandler.Handler(generator);

            var jfReference = (CmsCodeReference) CmsCodeFactory.JF(new CmsCode(generator.CodesLengh));
            generator.JFCodeReferenceStack.Push(jfReference);
            generator.AddCode(jfReference);

            generator.State = CmsCodeState.Initial;
        }
    }
}