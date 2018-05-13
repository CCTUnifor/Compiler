using System;
using System.Collections.Generic;
using System.Text;
using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Code.Instructions;

namespace MyCompiler.CodeGenerator.Code
{
    public static class CmsCodeFactory
    {
        public static CmsCode LSP(CmsCode reference) => new LSPCode(reference);
        public static CmsCode JMP(CmsCode reference) => new JMPCode(reference);
        public static CmsCode LDI => new LDICode();
        public static CmsCode ADI => new ADICode();
        public static CmsCode SUI => new SUICode();
        public static CmsCode MUI => new MUICode();
        public static CmsCode DVI => new DVICode();
        public static CmsCode LOD => new LODCode();
        public static CmsCode STO => new STOCode();
        public static CmsCode OUT => new OUTCode();
        public static CmsCode IN => new INCode();
        public static CmsCode JF => new JFCode();
        public static CmsCode EQ => new EQCode();
        public static CmsCode NE => new NECode();
        public static CmsCode GT => new GTCode();
        public static CmsCode GE => new GECode();
        public static CmsCode LT => new LTCode();
        public static CmsCode LE => new LECode();
        public static CmsCode STOP => new STOPCode();
    }
}
