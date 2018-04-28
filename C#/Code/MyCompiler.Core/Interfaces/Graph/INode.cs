using System.Collections.Generic;
using MyCompiler.Core.Enums.RegularExpression;

namespace MyCompiler.Core.Interfaces.Graph
{
    public abstract class INode
    {
        public readonly int Id;
        public ICollection<INodeAdjacent> AdjacentNodes { get; }
        public IEnumerable<INodeAdjacent> AdjacentNodesWithoutRepeat { get; }
        public bool IsRepeat { get; }
        public ILock Lock { get; private set; }

        protected INode(int id, bool isRepeat = false)
        {
            Id = id;
            AdjacentNodes = new List<INodeAdjacent>();
            IsRepeat = isRepeat;
        }

        public abstract void AddAdjacent(INode newEnd, IToken<RegularExpressionGrammarClass> token, bool isRepeat = false);
        public abstract string ToString();
        public abstract string ToStringAdjacents();
    }

    public interface ILock
    {
    }

    public abstract class INodeAdjacent : INode
    {
        public IToken<RegularExpressionGrammarClass> Token { get; set; }
        public bool IsBlank => Token.GrammarClass == RegularExpressionGrammarClass.Empty;
        public bool IsTerminal => Token.GrammarClass == RegularExpressionGrammarClass.Terminal;

        protected INodeAdjacent(int id, bool isRepeat = false) : base(id, isRepeat)
        {
        }
    }
}
