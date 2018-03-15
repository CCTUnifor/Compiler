using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Models.GraphModels;

namespace MyCompiler.Core.Models.ConstructionSubsets
{
    public class Lock
    {
        private readonly int Id;
        private readonly ICollection<Node> _nodes;

        public Lock(int id, IEnumerable<Node> nodes)
        {
            Id = id;
            _nodes = nodes.ToList();
        }

        public override string ToString()
        {
            var x = new List<string>();
            foreach (var node in _nodes)
                x.Add($"{node.Id}");

            return $"[lock({Id})] => {{{string.Join(", ", x)}}}";
        }
    }
}