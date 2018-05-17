using System.Threading.Tasks;
using AspectCore.DynamicProxy;

//using PostSharp.Aspects;
//using PostSharp.Serialization;

namespace MyCompiler.Parser.Aspects
{
    //[PSerializable]
    public class LogAnalyserAspectAttribute : AbstractInterceptorAttribute// OnMethodBoundaryAspect
    {
        //public override void OnEntry(MethodExecutionArgs args)
        //{
        //    Logger.IsToPrintInConsole = true;
        //    Logger.PrintHeader("Analyse the input:");
        //    base.OnEntry(args);
        //}


        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            throw new System.NotImplementedException();
        }
    }
}