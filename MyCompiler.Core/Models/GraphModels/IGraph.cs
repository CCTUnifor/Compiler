namespace MyCompiler.Core.Models.GraphModels
{
    public interface IGraph
    {
        Node Root { get; }
        Node End { get; }
        int NodeId { get; }

        Node AddToken(RegularExpressionToken token);
        IGraph AddSequence(IGraph sequenceGraph);
        IGraph AddChoice(IGraph concatGraph);
        bool IsEmpty { get; }
        void RepeatN();
        void RepeatPlus();
    }
}