using System;
using System.Collections.Generic;
using System.Text;
using MyCompiler.CodeGenerator.Code.Instructions;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar.Tokens.Terminals;

namespace MyCompiler.CodeGenerator.StatmentHandlers
{
    public class EndStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            var pop = generator.TokenStack.Pop();
            switch (pop)
            {
                case IfToken token:
                    generator.JFCodeReferenceStack.Pop().Reference.ValueDecimal = generator.CodesLengh;
                    break;
                case WhileToken token:
                    generator.AddCode(generator.StartWhileCodeReference.Pop());
                    generator.JFCodeReferenceStack.Pop().Reference.ValueDecimal = generator.CodesLengh;
                    break;
            }

            generator.MoveNextToken();
            generator.State = CmsCodeState.Initial;
        }
    }
}
