using MyCompiler.CodeGenerator.Code.Factories;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar.Tokens;
using MyCompiler.Grammar.Tokens.Terminals;
using MyCompiler.Tokenization.TopDown;

namespace MyCompiler.CodeGenerator.StatmentHandlers.TM
{
    internal class IfStatmentTMHandler : IStatmentTMHandler
    {
        public void Handler(TmCodeGenerator generator)
        {
            generator.Stack.Push(generator.Token);

            generator.RemoveParentheses<OpenParenthesesToken>();
            var reg = generator.VarDictionary[generator.Token];
            var line = generator.InstructionLine;

            generator.MoveNextToken();
            switch (generator.Token)
            {
                case GreatToken token:
                    var item = InstructionFactory.JLT(line, reg);
                    generator.Instructions.Add(item);
                    generator.IfBackpack.Push(new IfBackpackItem(line, item));
                    break;
                case GreatOrEqualToken token:
                    break;

                case LessToken token:
                    break;
                case LessOrEqualToken token:
                    break;

                case EqualToken token:
                    break;

                case NotEqualToken token:
                    break;
            }



            generator.RemoveParentheses<CloseParenthesesToken>();

            generator.GeneratorState = TinyCodeGeneratorState.Initial;
        }
    }

    public class IfBackpackItem
    {
        public int Line { get; set; }
        public string Command { get; set; }

        public IfBackpackItem(int line, string command)
        {
            Line = line;
            Command = command;
        }
    }
}

