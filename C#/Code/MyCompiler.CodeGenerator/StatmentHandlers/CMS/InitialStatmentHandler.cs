using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar.Tokens.Terminals;
using MyCompiler.Tokenization.TopDown;

namespace MyCompiler.CodeGenerator.StatmentHandlers.CMS
{
    public class InitialStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            switch (generator.Token)
            {
                case ReadToken token:
                    generator.GeneratorState = TinyCodeGeneratorState.Read;
                    break;
                case WriteToken token:
                    generator.GeneratorState = TinyCodeGeneratorState.Write;
                    break;
                case IfToken token:
                    generator.GeneratorState = TinyCodeGeneratorState.If;
                    generator.TokenStack.Push(generator.Token);
                    break;
                case WhileToken token:
                    generator.GeneratorState = TinyCodeGeneratorState.While;
                    generator.TokenStack.Push(generator.Token);
                    break;
                case AttributionToken token:
                    generator.GeneratorState = TinyCodeGeneratorState.Attribution;
                    break;
                case EndToken token:
                    generator.GeneratorState = TinyCodeGeneratorState.End;
                    break;
                case IdentifierToken token:
                    generator.MoveNextToken();
                    if (generator.Token is AttributionToken)
                    {
                        generator.AttributionTokenStack.Push(token);
                        generator.GeneratorState = TinyCodeGeneratorState.Attribution;
                    }
                    //else
                    //    generator.GeneratorState = TinyCodeGeneratorState.Identifier;
                    break;
                case RepeatToken token:
                    generator.GeneratorState = TinyCodeGeneratorState.Repeat;
                    //generator.TokenStack.Push(generator.Token);
                    break;
                case UntilToken token:
                    generator.GeneratorState = TinyCodeGeneratorState.Until;
                    break;
                default:
                    generator.MoveNextToken();
                    break;
            }
        }
    }
}
