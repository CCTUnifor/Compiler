using System;
using MyCompiler.Core.Interfaces;

namespace MyCompiler.Core.Models
{
    public class ThompsonConstruction : IThompsonConstruction
    {
        public Vertex ConcatStatment(Terminal a, Terminal b)
        {
            var simpleStatmentA = a.SimpleStatment();
            simpleStatmentA.GetEdge(0).End.AddEdge(b.Value.ToString(), new Vertex(3));

            return simpleStatmentA;
        }

        public void OrStatment()
        {
            throw new NotImplementedException();
        }

        public void RepetitionStatment()
        {
            throw new NotImplementedException();
        }
    }
}