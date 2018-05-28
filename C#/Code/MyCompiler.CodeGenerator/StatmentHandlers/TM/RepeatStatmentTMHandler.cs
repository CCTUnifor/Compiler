using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;

namespace MyCompiler.CodeGenerator.StatmentHandlers.TM
{
    public class RepeatStatmentTMHandler : IStatmentTMHandler
    {
        public void Handler(TmCodeGenerator generator)
        {
            generator.Stack.Push(generator.Token);
            generator.MoveNextToken();
            generator.MoveNextToken();
            var reg = generator.VarDictionary[generator.Token];
            var line = generator.InstructionLine;

            generator.RepeatBackpack.Push(new RepeatBackpack(line, reg));
            generator.GeneratorState = TinyCodeGeneratorState.Initial;
        }
    }

    public class RepeatBackpack
    {
        public int Line { get; }
        public int Reg { get; }

        public RepeatBackpack(int line, int reg)
        {
            Line = line;
            Reg = reg;
        }
    }
}