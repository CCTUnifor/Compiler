using System.Collections.Generic;
using System.Linq;

namespace MyCompiler.Core.Models
{
    public class Vertex
    {
        public int Number { get; }
        public ICollection<Edge> Edges { get; }

        public Vertex(int number)
        {
            Number = number;
            Edges = new List<Edge>();
        }

        public Vertex(int number, ICollection<Edge> edges)
        {
            Number = number;
            Edges = edges;
        }

        public void AddEdge(string identifier, Vertex x)
            => Edges.Add(new Edge(identifier, this, x));

        public Edge GetEdge(int i)
            => Edges.ToList()[i];

        public override string ToString()
            => $"({Number})";
    }
}