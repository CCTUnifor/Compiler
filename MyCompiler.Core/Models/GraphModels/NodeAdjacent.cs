using MyCompiler.Core.Enums.RegularExpression;

namespace MyCompiler.Core.Models.GraphModels
{
    public class NodeAdjacent : Node
    {
        public RegularExpressionToken Token { get; set; }
        public bool IsBlank => Token.GrammarClass == RegularExpressionGrammarClass.Empty;

        public NodeAdjacent(Node node, RegularExpressionToken token) : base(node.Id)
        {
            Token = token;
            AdjacentNodes = node.AdjacentNodes;
        }

        public override string ToString()
            => $"[{Id}]{{{Token}}}";
    }
}