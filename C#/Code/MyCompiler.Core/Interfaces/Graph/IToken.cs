using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompiler.Core.Interfaces.Graph
{
    public interface IToken
    {
        string Value { get; }

        bool IsEmpty();
        bool IsTerminal();
        bool IsIdentifier();
        bool IsNumber();
    }

    public interface ITerminalToken
    {

    }
}
