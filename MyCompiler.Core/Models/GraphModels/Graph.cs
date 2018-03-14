using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCompiler.Core.Models.GraphModels
{
    public class Graph : IGraph
    {
        public Node Root { get; private set; }
        public Node End { get; private set; }
        public static int NodeId { get; private set; }
        public bool IsEmpty { get; }

        public Graph()
        {
            Root = CreateNewNode();
            End = Root;
        }
        public Graph(RegularExpressionToken token)
        {
            Root = CreateNewNode();
            End = Root;

            AddToken(token);
        }

        public Node CreateNewNode()
        {
            IncrementNodeId();
            var node = new Node(NodeId);

            return node;
        }

        public Node AddToken(RegularExpressionToken token)
        {
            var newEnd = CreateNewNode();
            End.AddAdjacent(newEnd, token);
            End = newEnd;

            return End;
        }

        private void IncrementNodeId()
            => NodeId++;


        public IGraph AddSequence(IGraph sequenceGraph)
        {
            if (sequenceGraph == null)
                return this;
            End.AddAdjacent(sequenceGraph.Root, RegularExpressionToken.Blank);
            End = sequenceGraph.End;

            return this;
        }

        public IGraph AddChoice(IGraph concatGraph)
        {
            var newRoot = CreateNewNode();
            newRoot.AddAdjacent(Root, RegularExpressionToken.Blank);
            newRoot.AddAdjacent(concatGraph.Root, RegularExpressionToken.Blank);
            Root = newRoot;

            var newEnd = CreateNewNode();
            End.AddAdjacent(newEnd, RegularExpressionToken.Blank);
            concatGraph.End.AddAdjacent(newEnd, RegularExpressionToken.Blank);

            End = newEnd;

            return this;
        }

        public void RepeatN()
        {
            throw new System.NotImplementedException();
        }

        public void RepeatPlus()
        {
            throw new System.NotImplementedException();
        }

        public void Print()
        {
            var terminal = AllNodes(Root);
            foreach (var node in terminal)
                Console.WriteLine($"[{node.Id}] = > {node.ToStringAdjacents()}");
        }

        private static IEnumerable<Node> AllNodes(Node node)
        {
            var nodes = new List<Node> { node };

            foreach (var adj in node.AdjacentNodes)
                nodes.AddRange(AllNodes(adj));

            return nodes.Distinct();
        }
    }
}