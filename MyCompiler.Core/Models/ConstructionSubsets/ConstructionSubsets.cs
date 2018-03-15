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

        public IList<Lock> Generate(IGraph graph)
        {
            Graph = graph;
            var allLocks = FindAllLocks(Graph.Root);
            PrintAllLocks(allLocks);

            return allLocks.ToList();
        }

        private static ICollection<Lock> FindAllLocks(Node node)
        {
            var locks = new List<Lock> { new Lock(LockIncrement(), FindLock(node)) };

            foreach (var adj in node.AdjacentNodes)
                locks.AddRange(FindAllLocks(adj));

            return locks;
        }

        private static ICollection<Node> FindLock(Node node)
        {
            var nodes = new List<Node>() { node };
            foreach (var adj in node.AdjacentNodes)
            {
                if (adj.IsBlank)
                    nodes.AddRange(FindLock(adj));
            }

            return nodes;
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