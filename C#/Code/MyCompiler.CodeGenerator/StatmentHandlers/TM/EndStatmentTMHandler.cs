using System.Linq;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar.Tokens.Terminals;

namespace MyCompiler.CodeGenerator.StatmentHandlers.TM
{
    public class EndStatmentTMHandler : IStatmentTMHandler
    {
        public void Handler(TmCodeGenerator generator)
        {
            if (generator.Stack.Any())
            {
                var pop = generator.Stack.Pop();
                switch (pop)
                {
                    case IfToken token:
                        var ins = generator.Instructions.ToArray();
                        var ifBackpackItem = generator.IfBackpack.Pop();
                        ins[ifBackpackItem.Line] = ifBackpackItem.Command.Replace("lineJmp", generator.InstructionLine.ToString());
                        generator.Instructions = ins.ToList();
                        break;
                        //case WhileToken token:
                        //    generator.AddCode(generator.StartWhileCodeReference.Pop());
                        //    generator.JFCodeReferenceStack.Pop().Reference.ValueDecimal = generator.CodesLengh;
                        //    break;
                }
            }

            generator.MoveNextToken();
            generator.GeneratorState = TinyCodeGeneratorState.Initial;
        }
    }
}