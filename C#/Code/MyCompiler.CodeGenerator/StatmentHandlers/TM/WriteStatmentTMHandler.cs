using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar.Tokens.Terminals;

namespace MyCompiler.CodeGenerator.StatmentHandlers.TM
{
    internal class WriteStatmentTMHandler : IStatmentTMHandler
    {
        public void Handler(TmCodeGenerator generator)
        {
            generator.RemoveParentheses<OpenParenthesesToken>();
            generator.Instructions.Add(InstructionFactory.Write(generator.Instructions.Count, generator.VarDictionary[generator.Token]));
            generator.RemoveParentheses<CloseParenthesesToken>();

            generator.GeneratorState = TinyCodeGeneratorState.Initial;
        }
    }
}