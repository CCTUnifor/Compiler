namespace MyCompiler.Core.Models
{
    public class Edge
    {
        public string Identifier { get; private set; }
        public Vertex Start { get; private set; }
        public Vertex End { get; private set; }

        public Edge(string identifier, Vertex start, Vertex end)
        {
            Identifier = identifier;
            Start = start;
            End = end;
        }

        public override string ToString()
            => $"{Start} --- {Identifier} ---> {End} ";
    }
}