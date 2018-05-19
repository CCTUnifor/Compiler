using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCompiler.CodeGenerator.Code;

namespace MyCompiler.WebApp.ViewModels
{
    public class CodeViewModel
    {
        public string Position { get; set; }
        public CmsCode Code { get; set; }
    }
}
