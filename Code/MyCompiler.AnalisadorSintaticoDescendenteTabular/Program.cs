using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyCompiler.AnalisadorSintaticoDescendenteTabular
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("# Analisador Sintatico Descendente Tabular");
                var grammar = $"S -> XYZ\nX -> aXb | E\nY -> cYZcX | d\nZ -> eZYe | f";

                Console.WriteLine("\n++++++ Grammar ++++++\n");
                Console.WriteLine(grammar);
                Console.WriteLine("\n");

                First(grammar);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }

        private static void First(string grammar)
        {
            var lines = grammar.Split("\n");
            var firsts = new List<First>();// Dictionary<char, char[]>();

            Console.WriteLine("## Recorgnize the Grammar");
            foreach (var line in lines)
            {
                var caller = new NonTerminal(line.Split("->").FirstOrDefault().TrimEnd()[0]);
                var called = line.Split("->").LastOrDefault().TrimStart();
                var term = new Term(caller, called);

                if (firsts.All(x => x.NonTerminal != term.Caller))
                    firsts.Add(new First(term.Caller, new List<Terminal>()));

                foreach (var s in term.Called)
                {
                    var y = s.Trim();
                    if (IsTerminal(y[0]))
                    {
                        var first = firsts.Single(x => x.NonTerminal == term.Caller);
                        first.Terminals.Add(new Terminal(y[0]));
                    }
                }

                Console.WriteLine(term);
            }

            Console.WriteLine("\n## Firsts");
            foreach (var first in firsts)
                Console.WriteLine(first);
        }

        private static bool IsTerminal(char c) => char.IsLower(c);
    }

    public class First
    {
        public NonTerminal NonTerminal { get; }
        public ICollection<Terminal> Terminals { get; }

        public First(NonTerminal nonTerminal, ICollection<Terminal> terminals)
        {
            NonTerminal = nonTerminal;
            Terminals = terminals;
        }

        public override string ToString() => $"first({NonTerminal}) => [{string.Join(", ", Terminals)}]";
    }

    public class Terminal
    {
        public char Value { get; }
        public Terminal(char value) => Value = value;
        public override string ToString() => $"{Value}";
    }

    public class NonTerminal
    {
        public char Value { get; private set; }
        public NonTerminal(char value) => Value = value;
        public override string ToString() => $"{Value}";
    }

    public class Term
    {
        public NonTerminal Caller { get; }
        public string[] Called { get; }

        public Term(NonTerminal caller, string called)
        {
            Caller = caller;
            Called = called.Split("|");
        }

        public override string ToString() => $"{Caller} -> {string.Join(" | ", Called)}";
    }
}
