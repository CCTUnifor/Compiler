using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Core.Models.GraphModels;
using MyCompiler.Core.Models.Tokens;

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
                    adj.AddLock(_lock);
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

        public void PrintMatriz(IList<Lock> locks)
        {
            Console.WriteLine("Matriz");

            var terminais = GetTerminais(Graph.Root);
            PrintHeader(terminais);

            foreach (var _lock in locks)
            {
                Console.Write($"{_lock.Id.ToString().PadRight(2)} | ");
                foreach (var terminal in terminais)
                {
                    var node = _lock.NodeRef.Select(x => ((NodeAdjacent)x)).SingleOrDefault(y => y.Token.Value == terminal.Value);
                    if (node != null)
                        Console.Write(node.Lock.Id);
                    else
                        Console.Write("-");
                    Console.Write(" | ");

                }
                Console.WriteLine();
            }

            Console.WriteLine("\n-----------------------------------------------------\n");
        }

        private static void PrintHeader(IEnumerable<RegularExpressionToken> terminais)
        {
            Console.Write($"{"   | "}");
            foreach (var header in terminais)
            {
                Console.Write($"{header.Value} | ");
            }
            Console.WriteLine("");
        }

        private IEnumerable<RegularExpressionToken> GetTerminais(Node node)
        {
            var tokens = new List<RegularExpressionToken>();
            if ((node as NodeAdjacent)?.Token.GrammarClass == RegularExpressionGrammarClass.Terminal)
                tokens.Add(((NodeAdjacent)node).Token);

            foreach (var adj in node.AdjacentNodesWithoutRepeat)
            {
                var c = GetTerminais(adj);
                tokens.AddRange(c);
            }

            return tokens.GroupBy(x => x.Value).Select(x => x.First()).ToList();
        }
    }
}