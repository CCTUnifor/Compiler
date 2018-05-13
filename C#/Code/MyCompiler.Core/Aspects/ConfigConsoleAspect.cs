using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;

namespace MyCompiler.Core.Aspects
{
    public class ConfigConsoleAspect : AbstractInterceptorAttribute
    {
        //public override void OnEntry(MethodExecutionArgs args)
        //{
        //    Console.OutputEncoding = System.Text.Encoding.UTF8;
        //    ConsoleTableOptions.Pad = 60;
        //    ConsoleTableOptions.DefaultIfNull = "Error";
        //    Logger.PathToSave = $"Logs/log{DateTime.Now.Millisecond}.txt";

        //    Logger.PrintHeader("# Analisador Sintatico Descendente Tabular");
        //    base.OnEntry(args);
        //}
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}