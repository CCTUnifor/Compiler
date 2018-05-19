using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;

namespace MyCompiler.CodeGenerator.StatmentHandlers.CMS
{
    public class RepeatStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            generator.RepeatReferenceStack.Push(new CmsCode(generator.CodesLengh));
            generator.MoveNextToken();
            generator.GeneratorState = TinyCodeGeneratorState.Initial;
        }
    }
}