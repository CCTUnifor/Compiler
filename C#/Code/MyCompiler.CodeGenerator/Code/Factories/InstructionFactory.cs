using MyCompiler.CodeGenerator.Enums;

namespace MyCompiler.CodeGenerator.Code.Factories
{
    internal static class InstructionFactory
    {
        public static string Halt(int line) => $"{line}: {OpCodeTM.HALT} 0,0,0";

        public static string Read(int line, int reg) => $"{line}: {OpCodeTM.IN} {reg},0,0";

        public static string Write(int line, int reg) => $"{line}: {OpCodeTM.OUT} {reg},0,0";

        public static string JLT(int line, int reg) => $"{line}: {OpCodeTM.JLT} {reg},lineJmp(7)";
    }
}