using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar.Tokens.Terminals;

namespace MyCompiler.CodeGenerator.StatmentHandlers
{
    public class WriteStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            generator.RemoveParentheses<OpenParenthesesToken>();
            generator.AddCode(CmsCodeFactory.LOD(generator.VariableArea[generator.Token.Value]));
            generator.RemoveParentheses<CloseParenthesesToken>();

            generator.AddCode(CmsCodeFactory.OUT);
            generator.State = CmsCodeState.Initial;
        }
    }
}
