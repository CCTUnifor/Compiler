using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Models.GraphModels;

namespace MyCompiler.Core.Models.ConstructionSubsets
{
    public class ConstructionSubsets
    {
        public IGraph Graph { get; private set; }
        public static int LockId { get; private set; }
        public static int LockIncrement() => ++LockId;
        private ICollection<Node> NodesFindeds;

        public IList<Lock> Generate(IGraph graph)
        {
            Graph = graph;
            NodesFindeds = new List<Node>();

            var allLocks = FindAllLocks(Graph.Root);
            PrintAllLocks(allLocks);

            return allLocks.ToList();
        }

        private ICollection<Lock> FindAllLocks(Node node)
        {
            NodesFindeds.Add(node);

            var i = LockIncrement();
            var locks = new List<Lock>();

            var l = FindLock(node, i);
            locks.Add(l);

            foreach (var nodeRef in l.NodeRef.Where(x => !NodesFindeds.Contains(x)))
                locks = locks.Concat(FindAllLocks(nodeRef)).ToList();

            return locks;
        }

        private Lock FindLock(Node node, int i)
        {
            var _lock = new Lock(i);
            _lock.AddNode(node);

            foreach (var adj in node.AdjacentNodes)
            {
                if (adj.IsBlank)
                {
                    var _lockAux = FindLock(adj, i);
                    _lock.AddRangeNode(_lockAux.Nodes);
                    _lock.AddRangeNodeRef(_lockAux.NodeRef);
                }
                else
                {
                    _lock.AddNodeRef(adj);
                    break;
                }
            }

            return _lock;
        }

        private static void PrintAllLocks(ICollection<Lock> allLocks)
        {
            Console.WriteLine("All Locks");

            foreach (var _lock in allLocks)
                Console.WriteLine($"{_lock}");

            Console.WriteLine("\n-----------------------------------------------------\n");
        }
    }
}