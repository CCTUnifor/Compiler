using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar.Tokens.Terminals;
using MyCompiler.Tokenization.TopDown;

namespace MyCompiler.CodeGenerator.StatmentHandlers
{
    public class InitialStatmentHandler : IStatmentHandler
    {
        public void Handler(CmsCodeGenerator generator)
        {
            switch (generator.Token)
            {
                case ReadToken token:
                    generator.State = CmsCodeState.Read;
                    break;
                case WriteToken token:
                    generator.State = CmsCodeState.Write;
                    break;
                case IfToken token:
                    generator.State = CmsCodeState.If;
                    generator.TokenStack.Push(generator.Token);
                    break;
                case WhileToken token:
                    generator.State = CmsCodeState.While;
                    generator.TokenStack.Push(generator.Token);
                    break;
                case AttributionToken token:
                    generator.State = CmsCodeState.Attribution;
                    break;
                case EndToken token:
                    generator.State = CmsCodeState.End;
                    break;
                case IdentifierToken token:
                    generator.MoveNextToken();
                    if (generator.Token is AttributionToken)
                    {
                        generator.AttributionTokenStack.Push(token);
                        generator.State = CmsCodeState.Attribution;
                    }
                    //else
                    //    generator.State = CmsCodeState.Identifier;
                    break;
                case RepeatToken token:
                    generator.State = CmsCodeState.Repeat;
                    //generator.TokenStack.Push(generator.Token);
                    break;
                case UntilToken token:
                    generator.State = CmsCodeState.Until;
                    break;
                default:
                    generator.MoveNextToken();
                    break;
            }
        }
    }
}
