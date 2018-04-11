using System;
using CCTUnifor.ConsoleTable;
using CCTUnifor.Logger;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace MyCompiler.AnalisadorSintaticoDescendenteTabular.Aspects
{
    [PSerializable]
    public class ConfigConsoleAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ConsoleTableOptions.Pad = 60;
            ConsoleTableOptions.DefaultIfNull = "Error";
            Logger.PathToSave = $"Logs/log{DateTime.Now.Millisecond}.txt";

            Logger.PrintHeader("# Analisador Sintatico Descendente Tabular");
            base.OnEntry(args);
        }
    }
}