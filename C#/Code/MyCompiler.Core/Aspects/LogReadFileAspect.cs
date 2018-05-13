using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using CCTUnifor.Logger;
//using PostSharp.Aspects;
//using PostSharp.Serialization;

namespace MyCompiler.Core.Aspects
{
    //[PSerializable]
    public class LogReadFileAspect : AbstractInterceptorAttribute// OnMethodBoundaryAspect
    {
        //public override void OnExit(MethodExecutionArgs args)
        //{
        //    var type = (string)args.Arguments[0];
        //    var code = (string)args.ReturnValue;
        //    Logger.PrintHeader(type);
        //    Logger.PrintLn(code);

        //    base.OnExit(args);
        //}

        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            throw new System.NotImplementedException();
        }
    }
}