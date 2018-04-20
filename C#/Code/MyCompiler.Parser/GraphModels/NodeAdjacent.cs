using MyCompiler.Core.Enums.RegularExpression;
using MyCompiler.Grammar.Tokens;
using MyCompiler.Parser.ConstructionSubsets;

namespace MyCompiler.Parser.GraphModels
{
    public class NodeAdjacent : Node
    {
        public RegularExpressionToken Token { get; set; }
        public bool IsBlank => Token.GrammarClass == RegularExpressionGrammarClass.Empty;
        public bool IsTerminal => Token.GrammarClass == RegularExpressionGrammarClass.Terminal;
        public Lock Lock { get; private set; }

        public NodeAdjacent(Node node, RegularExpressionToken token, bool isRepeat) : base(node.Id, isRepeat)
        {
            Token = token;
            AdjacentNodes = node.AdjacentNodes;
        }

        public override string ToString()
            => $"[{Id}]{{{Token}}}";

        public void AddLock(Lock @lock)
        {
            if (Lock == null)
                Lock = @lock;
        }
    }
}