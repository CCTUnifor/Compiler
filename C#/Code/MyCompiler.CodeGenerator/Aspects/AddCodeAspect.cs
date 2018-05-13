using CCTUnifor.Logger;
using MyCompiler.CodeGenerator.Code;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace MyCompiler.CodeGenerator.Aspects
{
    [PSerializable]
    public class AddCodeAspect : OnMethodBoundaryAspect
    {
        public override void OnExit(MethodExecutionArgs args)
        {
            var codeAdded = (CmsCode) args.Arguments[0];
            Logger.PrintLn($"{codeAdded}");
            base.OnEntry(args);
        }
    }
}
