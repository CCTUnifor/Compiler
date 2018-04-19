using CCTUnifor.Logger;
using MyCompiler.Core.Models.Generators;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace MyCompiler.Core.Aspects
{
    [PSerializable]
    public class LogFirstAspect : OnMethodBoundaryAspect
    {
        public override void OnExit(MethodExecutionArgs args)
        {
            var model = (FirstGenerator)args.Instance;
            Logger.PrintHeader("Firsts");
            foreach (var first in model.Firsts)
                Logger.PrintLn(first.ToString());
            Logger.PrintLn("\n");

            base.OnExit(args);
        }
    }
}