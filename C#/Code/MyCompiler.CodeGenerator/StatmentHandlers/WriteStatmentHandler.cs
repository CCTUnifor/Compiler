using System;
using System.Collections.Generic;
using System.Text;
using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar;

namespace MyCompiler.CodeGenerator.StatmentHandlers
{
    public class WriteStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            generator.AddCode(CmsCodeFactory.LOD(generator.VariableArea[generator.Token.Value]));
            generator.AddCode(CmsCodeFactory.OUT);
            generator.State = CmsCodeState.Initial;
        }
    }
}
