using System;
using MyCompiler.Core.Interfaces.Graph;
using MyCompiler.Grammar.Extensions;
using MyCompiler.Grammar.Tokens;

namespace MyCompiler.Grammar
{
    public abstract class Token : IToken
    {
        public string Value { get; private set; }

        protected Token(string value) 
            => Value = value;

        public bool IsEmpty() => Value == "ε";
        public TerminalToken ToTerminalToken() => new TerminalToken(Value);
        public NonTerminalToken ToNonTerminalToken() => new NonTerminalToken(Value);
        public bool IsTerminal() => this is TerminalToken;
        public bool IsIdentifier() => Value.IsLetter();
        public bool IsNumber() => Value.IsNumber();

        public override string ToString() => $"{Value}";
        public override bool Equals(object obj) => ((Token)obj).Value == Value;
        protected bool Equals(Token other) => string.Equals(Value, other.Value);
        public override int GetHashCode() => (Value != null ? Value.GetHashCode() : 0);

        public static bool operator ==(Token token1, Token token2) => Equals(token1, token2);
        public static bool operator !=(Token token1, Token token2) => !Equals(token1, token2);

        public bool IsProgram() => Value.ToLower() == "program";
        public bool IsBegin() => Value.ToLower() == "begin";
        public bool IsVar() => Value.ToLower() == "var";
    }
}