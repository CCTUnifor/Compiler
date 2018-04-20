using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Interfaces;

namespace MyCompiler.Core.Models.SyntacticAnalyzes
{
    public class RegularExpressionParserToken<T>
    {
        private IEnumerable<IToken<T>> _tokens { get; set; }
        private ICollection<string> _events { get; set; }

        public RegularExpressionParserToken(IEnumerable<IToken<T>> tokens)
        {
            _tokens = tokens;
            _events = new List<string>();
        }

        public IToken<T> Peek
        {
            get
            {
                var x = _tokens.FirstOrDefault();
                AddEvent("Peek", x.ToString());
                return x;
            }
        }

        public void Eat(IToken<T> c)
        {
            if (Peek == c)
                _tokens = _tokens.Skip(1);
            else
                throw new Exception($"Expected: {c.Value}; got: {Peek}");
            AddEvent("Eat", c.ToString());
        }

        public void Eat(string c)
        {
            if (Peek.Value == c)
                _tokens = _tokens.Skip(1).ToArray();
            else
                throw new Exception($"Expected: '{c}'; got: {Peek}");

            AddEvent("Eat", $"'{c}'");
        }

        public IToken<T> Next()
        {
            var c = Peek;
            Eat(c);
            return c;
        }

        public bool More()
            => _tokens.Any();

        private void AddEvent(string method, string c)
            => _events.Add($"{method.PadRight(10)} {c}");

        public void PrintEvents()
        {
            Console.WriteLine("\nPrint Regular Expression Parser Events");
            foreach (var e in _events)
                Console.WriteLine(e);
        }

        public override string ToString()
            => string.Join(", ", _tokens);
    }
}