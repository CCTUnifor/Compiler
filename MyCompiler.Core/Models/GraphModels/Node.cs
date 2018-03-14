using System.Collections.Generic;

namespace MyCompiler.Core.Models.GraphModels
{
    public class Node
    {
        public readonly int Id;
        public ICollection<NodeAdjacent> AdjacentNodes { get; protected set; }

        public Node(int id)
        {
            Id = id;
            AdjacentNodes = new List<NodeAdjacent>();
        }

        public void AddAdjacent(Node newEnd, RegularExpressionToken token)
        {
            var adj = new NodeAdjacent(newEnd, token);
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