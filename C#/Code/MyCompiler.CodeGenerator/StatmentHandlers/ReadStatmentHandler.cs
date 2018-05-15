using System;
using System.Collections.Generic;
using System.Text;
using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar;
using MyCompiler.Grammar.Tokens;

namespace MyCompiler.CodeGenerator.StatmentHandlers
{
    public class ReadStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            generator.AddCode(CmsCodeFactory.IN);

            generator.Token = generator.Tokenization.GetTokenIgnoreSpace();
            generator.AddCode(CmsCodeFactory.STO(generator.VariableArea[generator.Token.Value]));
            generator.Token = generator.Tokenization.GetTokenIgnoreSpace();

            generator.State = CmsCodeState.Initial;
        }
    }
}
