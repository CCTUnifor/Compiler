using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Interfaces;

namespace MyCompiler.Core.Models.SyntacticAnalyzes
{
    public class RegularExpressionParserToken<T>
    {
        private IEnumerable<IToken<T>> _tokens { get; set; }

        public RegularExpressionParserToken(IEnumerable<IToken<T>> tokens)
        {
            _tokens = tokens;
        }

        public IToken<T> Peek
            => _tokens.FirstOrDefault();

        public void Eat(IToken<T> c)
        {
            if (Peek == c)
                _tokens = _tokens.Skip(1);
            else
                throw new Exception($"Expected: {c.Line}; got: {Peek.Line}");
        }

        public IToken<T> Next()
        {
            var c = Peek;
            Eat(c);
            return c;
        }

        public bool More()
            => _tokens.Any();
    }
}