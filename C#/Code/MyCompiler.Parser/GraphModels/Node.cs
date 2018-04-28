using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Interfaces.Graph;
using MyCompiler.Grammar.Tokens;

namespace MyCompiler.Parser.GraphModels
{
    public class Node : INode
    {
        public readonly int Id;
        public ICollection<INodeAdjacent> AdjacentNodes { get; protected set; }
        public IEnumerable<INodeAdjacent> AdjacentNodesWithoutRepeat => AdjacentNodes.Where(x => !x.IsRepeat);

        public override bool IsRepeat { get; private set; }

        public Node(int id, bool isRepeat = false)
        {
            Id = id;
            AdjacentNodes = new List<NodeAdjacent>();
            IsRepeat = isRepeat;
        }

        public void AddAdjacent(Node newEnd, RegularExpressionToken token, bool isRepeat = false)
        {
            var adj = new NodeAdjacent(newEnd, token, isRepeat);
            AdjacentNodes.Add(adj);
        }

        public override string ToString()
            => $"[{Id}]";

        public override string ToStringAdjacents()
        {
            return string.Join(", ", AdjacentNodes);
        }
    }
}