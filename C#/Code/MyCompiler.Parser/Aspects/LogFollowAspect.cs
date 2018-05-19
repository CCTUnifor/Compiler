using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;

//using PostSharp.Aspects;
//using PostSharp.Serialization;

namespace MyCompiler.Parser.Aspects
{
    //[PSerializable]
    public class LogFollowAspect : AbstractInterceptorAttribute// OnMethodBoundaryAspect
    {
        //public override void OnExit(MethodExecutionArgs args)
        //{
        //    var model = (FollowGenerator)args.Instance;
        //    Logger.PrintHeader("Follows");
        //    foreach (var follow in model.Follows)
        //        Logger.PrintLn(follow.ToString());
        //    Logger.PrintLn("\n");

        //    base.OnExit(args);
        //}
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            throw new System.NotImplementedException();
        }
    }
}