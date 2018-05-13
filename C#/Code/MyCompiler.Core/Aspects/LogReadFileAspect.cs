using CCTUnifor.Logger;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace MyCompiler.Core.Aspects
{
    [PSerializable]
    public class LogReadFileAspect : OnMethodBoundaryAspect
    {
        public override void OnExit(MethodExecutionArgs args)
        {
            var type = (string)args.Arguments[0];
            var code = (string)args.ReturnValue;
            Logger.PrintHeader(type);
            Logger.PrintLn(code);

            base.OnExit(args);
        }
    }
}