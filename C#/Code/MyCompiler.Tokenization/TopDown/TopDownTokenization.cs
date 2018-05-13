using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Enums;
using MyCompiler.Grammar;
using MyCompiler.Grammar.Extensions;
using MyCompiler.Grammar.Tokens;

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
                    case LexicAnalyserState.Letter:
                        if (IsNonTerminal() && NextIsSpaceOrNewline())
                            State = LexicAnalyserState.NonTerminal;
                        else if (Value.IsEmpty())
                            State = LexicAnalyserState.Empty;
                        else if (!IsNonTerminal() && NextIsSpaceOrNewline())
                            State = LexicAnalyserState.Terminal;
                        else if (IsSpace())
                            State = LexicAnalyserState.Space;
                        else
                        {
                            CurrentIndex++;
                            Value += Character;
                        }
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
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return token;
        }

        private bool IsSpace() => Character == ' ';

        private void HandleInitial()
        {
            if (IsNonTerminal())
                State = LexicAnalyserState.NonTerminal;
            else if (IsSpace())
                State = LexicAnalyserState.Space;
            else if (IsNewLine(Value))
                State = LexicAnalyserState.NewLine;
            else
                State = LexicAnalyserState.Letter;
        }

        private bool IsNonTerminal() =>
            _nonTerminals.OrderByDescending(x => x.Value.Length).Any(x => x.Value == Value);
        private bool IsNewLine(string v) => v == "\r" || v == "\t" || v == "\n";
        private bool NextIsSpaceOrNewline() => NextCharacter == ' ' || IsNewLine(NextCharacter.ToString());
    }
}