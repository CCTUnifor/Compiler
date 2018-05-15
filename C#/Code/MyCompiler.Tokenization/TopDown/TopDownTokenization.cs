using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Enums;
using MyCompiler.Grammar;
using MyCompiler.Grammar.Extensions;
using MyCompiler.Grammar.Tokens;
using MyCompiler.Grammar.Tokens.Terminals;

namespace MyCompiler.Tokenization.TopDown
{
    public class TopDownTokenization
    {
        private readonly IEnumerable<NonTerminalToken> _nonTerminals;
        private readonly string _production;

        private int CurrentIndex { get; set; }
        private LexicAnalyserState State { get; set; }

        private char Character => _production[CurrentIndex];
        private char NextCharacter => CurrentIndex + 1 < _production.Length ? _production[CurrentIndex + 1] : ' ';
        private string Value;
        public bool HasNext => CurrentIndex < _production.Length;

        public TopDownTokenization(IEnumerable<NonTerminalToken> nonTerminals, string production)
        {
            _nonTerminals = nonTerminals;
            _production = production;
        }

        public Token GetTokenIgnoreSpace()
        {
            var token = GetToken();
            while (token != null && token is SpaceToken || token is NewLineToken)
                token = GetToken();
            return token;
        }

        public IEnumerable<Token> GetAllTokens()
        {
            var tokens = new List<Token>();
            var currentAux = CurrentIndex;
            CurrentIndex = 0;

            while (HasNext)
                tokens.Add(GetToken());

            CurrentIndex = currentAux;
            return tokens;
        }

        public Token GetToken()
        {
            State = LexicAnalyserState.Initial;
            Value = HasNext ? Character.ToString() : "";
            Token token = null;

            while (HasNext && token == null)
            {
                switch (State)
                {
                    case LexicAnalyserState.Initial:
                        HandleInitial();
                        break;

                    case LexicAnalyserState.NonTerminal:
                        token = new NonTerminalToken(Value);
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Space:
                        token = new SpaceToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Terminal:
                        token = new TerminalToken(Value);
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Empty:
                        token = new EmptyToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.NewLine:
                        token = new NewLineToken(Value);
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Identifier:
                        token = new IdentifierToken(Value);
                        CurrentIndex++;
                        break;
                    case LexicAnalyserState.Number:
                        token = new NumberToken(Value);
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Var:
                        token = new VarToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Write:
                        token = new WriteToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Read:
                        token = new ReadToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.If:
                        token = new IfToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.End:
                        token = new EndToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Then:
                        token = new ThenToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Begin:
                        token = new BeginToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.While:
                        token = new WhileToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Do:
                        token = new DoToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.SemiColon:
                        token = new SemiColonToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.OpenParentheses:
                        token = new OpenParenthesesToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.CloseParentheses:
                        token = new CloseParenthesesToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Plus:
                        token = new PlusToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Sub:
                        token = new SubToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Great:
                        token = new GreatToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Less:
                        token = new LessToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Equal:
                        token = new EqualToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Repeat:
                        token = new RepeatToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Until:
                        token = new UntilToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.Attribution:
                        token = new AttributionToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.NotEqual:
                        token = new NotEqualToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.GreatOrEqual:
                        token = new GreatOrEqualToken();
                        CurrentIndex++;
                        break;

                    case LexicAnalyserState.LessOrEqual:
                        token = new LessOrEqualToken();
                        CurrentIndex++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return token;
        }

        private bool IsSpace() => Character == ' ';

        private void HandleInitial()
        {
            if (IsNonTerminal() && NextIsSpaceOrNewline())
                State = LexicAnalyserState.NonTerminal;
            else if (IsSpace())
                State = LexicAnalyserState.Space;
            else if (IsNewLine(Value))
                State = LexicAnalyserState.NewLine;
            else
            {
                switch (Value.ToLower())
                {
                    case "var":
                        MoveNextIfNextIsSpace(LexicAnalyserState.Var);
                        break;
                    case "write":
                        MoveNextIfNextIsSpace(LexicAnalyserState.Write);
                        break;
                    case "read":
                        MoveNextIfNextIsSpace(LexicAnalyserState.Read);
                        break;
                    case "if":
                        State = LexicAnalyserState.If;
                        break;
                    case "end":
                        State = LexicAnalyserState.End;
                        break;
                    case "then":
                        State = LexicAnalyserState.Then;
                        break;
                    case "begin":
                        State = LexicAnalyserState.Begin;
                        break;
                    case "while":
                        MoveNextIfNextIsSpace(LexicAnalyserState.While);
                        break;
                    case "do":
                        State = LexicAnalyserState.Do;
                        break;
                    case ";":
                        State = LexicAnalyserState.SemiColon;
                        break;
                    case "(":
                        State = LexicAnalyserState.OpenParentheses;
                        break;
                    case ")":
                        State = LexicAnalyserState.CloseParentheses;
                        break;
                    case "+":
                        State = LexicAnalyserState.Plus;
                        break;
                    case "-":
                        State = LexicAnalyserState.Sub;
                        break;
                    case ">":
                        if (NextCharacter == '=')
                            State = LexicAnalyserState.GreatOrEqual;
                        else
                            State = LexicAnalyserState.Great;
                        break;
                    case "<":
                        if (NextCharacter == '=')
                            State = LexicAnalyserState.LessOrEqual;
                        else
                            State = LexicAnalyserState.Less;
                        break;
                    case "=":
                        State = LexicAnalyserState.Equal;
                        break;
                    case "!=":
                        State = LexicAnalyserState.NotEqual;
                        break;
                    case "ε":
                        State = LexicAnalyserState.Empty;
                        break;
                    case "repeat":
                        State = LexicAnalyserState.Repeat;
                        break;
                    case "until":
                        State = LexicAnalyserState.Until;
                        break;
                    case ":=":
                        State = LexicAnalyserState.Attribution;
                        break;
                    default:
                        MoveNext();
                        break;
                }

            }
        }

        private void MoveNext()
        {
            if (char.IsLetter(Value[0]) && NextIsSpaceOrNewline())
                State = LexicAnalyserState.Identifier;
            else if (Value.IsNumber() && NextIsSpaceOrNewline())
                State = LexicAnalyserState.Number;
            else if (NextIsSpaceOrNewline())
                State = LexicAnalyserState.Terminal;
            else
            {
                State = LexicAnalyserState.Initial;
                CurrentIndex++;
                if (HasNext)
                    Value += Character;
            }
        }

        private void MoveNextIfNextIsSpace(LexicAnalyserState state)
        {
            if (NextIsSpaceOrNewline())
                State = state;
            else
                MoveNext();
        }

        private bool IsNonTerminal() =>
            _nonTerminals.OrderByDescending(x => x.Value.Length).Any(x => x.Value == Value);
        private bool IsNewLine(string v) => v == "\r" || v == "\t" || v == "\n";
        private bool NextIsSpaceOrNewline() => NextCharacter == ' ' || IsNewLine(NextCharacter.ToString());
    }

    public class LessOrEqualToken : TerminalToken
    {
        public LessOrEqualToken() : base("<=")
        {
        }
    }

    public class GreatOrEqualToken : TerminalToken
    {
        public GreatOrEqualToken() : base(">=")
        {
        }
    }

    public class NotEqualToken : TerminalToken
    {
        public NotEqualToken() : base("!=")
        {
        }
    }

    public class AttributionToken : TerminalToken
    {
        public AttributionToken() : base(":=")
        {
        }
    }
}