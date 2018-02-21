namespace MyCompiler.Core.Models
{
    public class Terminal
    {
        public char Value { get; }
        public bool IsTerminal => char.IsLetter(Value) && char.IsLower(Value);

        public Terminal(char value)
            => Value = value;

        public Vertex SimpleStatment()
        {
            var vertex = new Vertex(1);
            vertex.AddEdge(Value.ToString(), new Vertex(2));

            return vertex;
        }
    }
}