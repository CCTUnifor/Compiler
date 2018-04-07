using CCTUnifor.Logger;
using MyCompiler.Core.Models.SyntacticAnalyzes.NRDSA;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace MyCompiler.Core.Aspects
{
    [PSerializable]
    public class LogFollowAspect : OnMethodBoundaryAspect
    {
        public override void OnExit(MethodExecutionArgs args)
        {
            var model = (NonRecursiveDescendingSyntacticAnalysis)args.Instance;
            Logger.PrintHeader("Follows");
            foreach (var follow in model.Follows)
                Logger.PrintLn(follow.ToString());
            Logger.PrintLn("\n");

            base.OnExit(args);
        }
    }
}