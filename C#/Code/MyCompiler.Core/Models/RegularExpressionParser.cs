using System;
using System.Linq;

namespace MyCompiler.Core.Models
{
    public class RegularExpressionParser
    {
        private string _input { get; set; }

        public RegularExpressionParser(string input)
        {
            _input = input;
        }

        public char Peek()
            => _input[0];

        public void Eat(char c)
        {
            if (Peek() == c)
                _input = _input.Substring(1);
            else
                throw new Exception($"Expected: {c}; got: {Peek()}");
        }

        public char Next()
        {
            var c = Peek();
            Eat(c);
            return c;
        }

        public bool More()
            => _input.Any();
    }
}