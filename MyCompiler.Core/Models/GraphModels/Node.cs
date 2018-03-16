using System.Collections.Generic;
using System.Linq;

namespace MyCompiler.Core.Models.GraphModels
{
    public class Node
    {
        public readonly int Id;
        public ICollection<NodeAdjacent> AdjacentNodes { get; protected set; }
        public IEnumerable<NodeAdjacent> AdjacentNodesWithoutRepeat => AdjacentNodes.Where(x => !x.IsRepeat);

        public bool IsRepeat { get; private set; }

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

        public string ToStringAdjacents()
        {
            return string.Join(", ", AdjacentNodes);
        }
    }
}