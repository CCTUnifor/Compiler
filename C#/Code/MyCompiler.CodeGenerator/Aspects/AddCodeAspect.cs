using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using CCTUnifor.Logger;
using MyCompiler.CodeGenerator.Code;
//using PostSharp.Aspects;
//using PostSharp.Serialization;

namespace MyCompiler.CodeGenerator.Aspects
{
    //[PSerializable]
    public class AddCodeAspect : AbstractInterceptorAttribute//OnMethodBoundaryAspect
    {
        //public override void OnExit(MethodExecutionArgs args)
        //{
        //    var codeAdded = (CmsCode) args.Arguments[0];
        //    Logger.PrintLn($"{codeAdded}");
        //    base.OnEntry(args);
        //}

        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            throw new System.NotImplementedException();
        }
    }
}
