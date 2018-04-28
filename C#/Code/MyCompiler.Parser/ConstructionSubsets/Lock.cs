using System.Collections.Generic;
using System.Linq;
using MyCompiler.Parser.GraphModels;

namespace MyCompiler.Parser.ConstructionSubsets
{
    public class Lock
    {
        public readonly int Id;
        public ICollection<Node> Nodes { get; private set; }
        public ICollection<Node> NodeRef { get; private set; }

        public Lock(int id)
        {
            Id = id;
            Nodes = new List<Node>();
            NodeRef = new List<Node>();
        }

        public Lock(int id, IEnumerable<Node> nodes)
        {
            Id = id;
            Nodes = nodes.ToList();
        }

        public void AddNode(Node node) => Nodes.Add(node);
        public void AddRangeNode(IEnumerable<Node> node) => Nodes = Nodes.Concat(node).ToList();

        public void AddNodeRef(Node adj) => NodeRef.Add(adj);
        public void AddRangeNodeRef(IEnumerable<Node> adj) => NodeRef = NodeRef.Concat(adj).ToList();

        public override string ToString()
        {
            var x = new List<string>();
            var y = new List<string>();
            foreach (var node in Nodes)
                x.Add($"{node.Id}");

            foreach (var node in NodeRef)
                y.Add($"    lock ε({Id}, {((NodeAdjacent)node).Token.Value}) => {{ {node.Id} }}");

            return $"lock({Id}) => {{ {string.Join(", ", x)} }}\n" +
                    $"{string.Join("    ", y)}";
        }

    }
}