using System.Collections.Generic;
using MyCompiler.Core.Models;

namespace MyCompiler.Core.Interfaces
{
    public interface IThompsonConstruction
    {
        Vertex ConcatStatment(Terminal a, Terminal b);
        void OrStatment();
        void RepetitionStatment();
    }
}