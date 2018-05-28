using System.Linq;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar.Tokens.Terminals;

namespace MyCompiler.CodeGenerator.StatmentHandlers.CMS
{
    public class EndStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            if (generator.TokenStack.Any())
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
            }

            generator.MoveNextToken();
            generator.GeneratorState = TinyCodeGeneratorState.Initial;
        }
    }
}
