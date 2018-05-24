using MyCompiler.CodeGenerator.Code.Factories;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar.Tokens.Terminals;

namespace MyCompiler.CodeGenerator.StatmentHandlers.CMS
{
    public class ReadStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            generator.AddCode(CmsCodeFactory.IN);

            generator.RemoveParentheses<OpenParenthesesToken>();
            generator.AddCode(CmsCodeFactory.STO(generator.VariableArea[generator.Token.Value]));
            generator.RemoveParentheses<CloseParenthesesToken>();

            generator.GeneratorState = TinyCodeGeneratorState.Initial;
        }


    }
}
