using MyCompiler.Core.Enums.RegularExpression;

namespace MyCompiler.Core.Interfaces.Graph
{
    public interface IGraph
    {
        INode Root { get; }
        INode End { get; }

        INode AddToken(_IToken<RegularExpressionGrammarClass> token);
        IGraph AddSequence(IGraph sequenceGraph);
        IGraph AddChoice(IGraph concatGraph);
        bool IsEmpty { get; }
        void RepeatN();
        void RepeatPlus();
        void Print();
    }
}