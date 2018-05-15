using System;
using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;

namespace MyCompiler.CodeGenerator.StatmentHandlers
{
    public class AttributionStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            var c = generator.Token;
            var exp = new ExpressionStatmentHandler();
            exp.Handler(generator);

            generator.AddCode(CmsCodeFactory.STO(generator.VariableArea[c.Value]));
            generator.State = CmsCodeState.Initial;
        }
    }
}