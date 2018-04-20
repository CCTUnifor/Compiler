﻿using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Enums;
using MyCompiler.Core.Extensions;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Core.Models.LexicalAnalyzes
{
    public class TopDownParser
    {
        private readonly IEnumerable<NonTerminalToken> _nonTerminals;
        private readonly string _production;

        private int CurrentIndex { get; set; }
        private LexicAnalyserState State { get; set; }

        private char Character => _production[CurrentIndex];
        private char NextCharacter => CurrentIndex + 1 < _production.Length ? _production[CurrentIndex + 1] : ' ';
        private string Value;
        private bool Continue => CurrentIndex < _production.Length;

        public TopDownParser(IEnumerable<NonTerminalToken> nonTerminals, string production)
        {
            _nonTerminals = nonTerminals;
            _production = production;
        }

        public Token GetToken()
        {
            State = LexicAnalyserState.Initial;
            Value = Continue ? Character.ToString() : "";
            Token token = null;

            while (Continue && token == null)
            {
                switch (State)
                {
                    case LexicAnalyserState.Initial:
                        HandleInitial();
                        break;
                    case LexicAnalyserState.Letter:
                        if (IsNonTerminal() && NextCharacter == ' ')
                            State = LexicAnalyserState.NonTerminal;
                        else if (Value.IsEmpty())
                            State = LexicAnalyserState.Empty;
                        else if (!IsNonTerminal() && NextCharacter == ' ')
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
            else
                State = LexicAnalyserState.Letter;
        }

        private bool IsNonTerminal() =>
            _nonTerminals.OrderByDescending(x => x.Value.Length).Any(x => x.Value == Value);
    }
}