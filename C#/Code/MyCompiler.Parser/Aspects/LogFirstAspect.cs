using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
//using PostSharp.Aspects;
//using PostSharp.Serialization;

namespace MyCompiler.Parser.Aspects
{
    //[PSerializable]
    public class LogFirstAspect : AspectCore.DynamicProxy.AbstractInterceptorAttribute//OnMethodBoundaryAspect
    {
        //public override void OnExit(MethodExecutionArgs args)
        //{
        //    var model = (FirstGenerator)args.Instance;
        //    Logger.PrintHeader("Firsts");
        //    foreach (var first in model.Firsts)
        //        Logger.PrintLn(first.ToString());
        //    Logger.PrintLn("\n");

        //    base.OnExit(args);
        //}
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            throw new System.NotImplementedException();
        }
    }
}