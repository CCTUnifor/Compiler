namespace MyCompiler.CodeGenerator.Enums
{
    public enum Instruction
    {
        LSP,
        JMP,
        LDI,
        ADI,
        SUI,
        MUI,
        DVI,
        LOD,
        STO,
        OUT,
        IN,
        JF,
        EQ, // =
        NE, // diferente
        GT, // >
        GE, // >=
        LT, // <
        LE, //<=
        STOP,
        Reference
    }
}