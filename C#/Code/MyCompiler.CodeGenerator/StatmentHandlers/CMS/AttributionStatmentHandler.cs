using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;

namespace MyCompiler.CodeGenerator.StatmentHandlers.CMS
{
    public class AttributionStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            var exp = new ExpressionStatmentHandler();
            exp.Handler(generator);

            var token = generator.AttributionTokenStack.Pop();
            generator.AddCode(CmsCodeFactory.STO(generator.VariableArea[token.Value]));
            generator.GeneratorState = TinyCodeGeneratorState.Initial;
        }
    }
}