using CCTUnifor.Logger;
using MyCompiler.Core.Models.Generators;
using PostSharp.Aspects;
using PostSharp.Serialization;
using FollowGenerator = MyCompiler.Tokenization.Generators.FollowGenerator;

namespace MyCompiler.Tokenization.Aspects
{
    [PSerializable]
    public class LogFollowAspect : OnMethodBoundaryAspect
    {
        public override void OnExit(MethodExecutionArgs args)
        {
            var model = (FollowGenerator)args.Instance;
            Logger.PrintHeader("Follows");
            foreach (var follow in model.Follows)
                Logger.PrintLn(follow.ToString());
            Logger.PrintLn("\n");

            base.OnExit(args);
        }
    }
}