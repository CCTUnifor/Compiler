using System;
using System.Collections.Generic;
using System.Text;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar;

namespace MyCompiler.CodeGenerator.StatmentHandlers
{
    public class InitialStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            switch (generator.Token.Value.ToLower())
            {
                case "read":
                    generator.State = CmsCodeState.Read;
                    break;
                case "write":
                    generator.State = CmsCodeState.Write;
                    break;
                case "if":
                    generator.State = CmsCodeState.If;
                    generator.TokenStack.Push(generator.Token);
                    break;
                case "end":
                    generator.State = CmsCodeState.End;
                    break;
            }
        }
    }
}
