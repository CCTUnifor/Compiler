using MyCompiler.CodeGenerator.Code.Instructions;

namespace MyCompiler.CodeGenerator.Code.Factories
{
    public static class CmsCodeFactory
    {
        public static CmsCode LSP(CmsCode reference) => new LSPCode(reference);
        public static CmsCode JMP(CmsCode reference) => new JMPCode(reference);
        public static CmsCode LDI(CmsCode reference) => new LDICode(reference);
        public static CmsCode ADI(CmsCode reference) => new ADICode(reference);
        public static CmsCode SUI => new SUICode();
        public static CmsCode MUI => new MUICode();
        public static CmsCode DVI => new DVICode();
        public static CmsCode LOD(CmsCode reference) => new LODCode(reference);
        public static CmsCode STO(CmsCode reference) => new STOCode(reference);
        public static CmsCode OUT => new OUTCode();
        public static CmsCode IN => new INCode();
        public static CmsCode JF(CmsCode reference) => new JFCode(reference);
        public static CmsCode EQ => new EQCode();
        public static CmsCode NE => new NECode();
        public static CmsCode GT => new GTCode();
        public static CmsCode GE => new GECode();
        public static CmsCode LT => new LTCode();
        public static CmsCode LE => new LECode();
        public static CmsCode STOP => new STOPCode();
    }
}
