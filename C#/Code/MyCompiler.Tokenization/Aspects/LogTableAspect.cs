using System.Collections.Generic;
using System.Linq;
using CCTUnifor.ConsoleTable;
using CCTUnifor.Logger;
using MyCompiler.Core.Models.Generators;
using PostSharp.Aspects;
using PostSharp.Serialization;
using TableGenerator = MyCompiler.Tokenization.Generators.TableGenerator;

namespace MyCompiler.Tokenization.Aspects
{
    [PSerializable]
    public class LogTableAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Logger.IsToPrintInConsole = false;
            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            var model = (TableGenerator)args.Instance;
            Logger.PrintHeader("Table");

            var collumnsHeader = model.Terminals.Select(x => x.Value).ToArray();
            var rowsHeader = model.NonTerminals.Select(x => x.Value.ToString()).ToArray();

            var tab = new ConsoleTable(collumnsHeader, rowsHeader);
            for (var i = 0; i < model.NonTerminals.Count(); i++)
            {
                var zxc = new List<Term>();
                for (var j = 0; j < model.Terminals.Count(); j++)
                {
                    zxc.Add(model.Table[i, j]);
                }
                tab.AddRow(zxc.Select(x => x?.ToString() ?? "").ToArray());
            }

            tab.Write();

            Logger.PrintLn("\n");
            base.OnExit(args);
        }
    }
}