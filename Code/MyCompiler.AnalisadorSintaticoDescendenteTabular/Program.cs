using System;
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
                var grammar = $"D -> TX\n" +
                              $"X -> +TX | -TX | E\n" +
                              $"T -> FY\n" +
                              $"Y -> *FY | %FY | E\n" +
                              $"F -> (E) | ide | num";


                Console.WriteLine("\n++++++ Grammar ++++++\n");
                Console.WriteLine(grammar);
                Console.WriteLine("\n");

                var syntacticAnalysis = new NonRecursiveDescendingSyntacticAnalysis(grammar);
                syntacticAnalysis.Parser();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }


    }

    public class NonRecursiveDescendingSyntacticAnalysis
    {
        public string Grammar { get; }
        public ICollection<Term> Terms { get; private set; }
        public ICollection<First> Firsts { get; private set; }

        private static bool IsTerminal(char c) => !IsNonTerminal(c);
        private static bool IsNonTerminal(char c) => char.IsLetter(c) && char.IsUpper(c);

        private static bool IsTerminal(string c) => !IsNonTerminal(c) && c.All(char.IsLetter);
        private static bool IsNonTerminal(string c) => c.All(x => char.IsLetter(x) && char.IsUpper(x));

        private static bool IsEmpty(char c) => c == 'E';
        private static bool IsEmpty(string c) => c == "E";

        public NonRecursiveDescendingSyntacticAnalysis(string grammar)
        {
            Grammar = grammar;
            Terms = new List<Term>();
            Firsts = new List<First>();
        }

        public void Parser()
        {
            GenerateFirst();
        }

        private void GenerateFirst()
        {
            var lines = Grammar.Split("\n");

            Console.WriteLine("## Recorgnize the Grammar");
            foreach (var line in lines)
            {
                var caller = new NonTerminal(line.Split("->").FirstOrDefault().TrimEnd()[0]);
                var called = line.Split("->").LastOrDefault().TrimStart();

                var term = new Term(caller, called);
                Terms.Add(term);

                InitializeFirst(term);

                Console.WriteLine(term);
            }
            foreach (var term in Terms)
                First(term);

            Console.WriteLine("\n## Firsts");
            foreach (var first in Firsts)
                Console.WriteLine(first);
        }

        private First First(Term term)
        {

            First f = null;
            foreach (var s in term.Derivations)
            {
                var nonTerminas = s.Trim();
                var firstElement = nonTerminas.FirstOrDefault();

                if (IsTerminal(nonTerminas))
                    f = AddFirst(term, nonTerminas);
                else if (IsTerminal(firstElement) || IsEmpty(firstElement))
                    f = AddFirst(term, firstElement.ToString());
                else
                {
                    var termDerivated = Terms.SingleOrDefault(x => x.Caller.Value == firstElement);
                    f = AddFirst(term, First(termDerivated));

                    foreach (var nonTerminal in nonTerminas.Remove(0, 1))
                    {
                        termDerivated = Terms.SingleOrDefault(x => x.Caller.Value == nonTerminal);

                        if (IsEmpty(termDerivated.Derivations.FirstOrDefault()))
                            f = AddFirst(term, First(termDerivated));
                    }
                }
            }

            return f;
        }

        private First AddFirst(Term term, First first)
        {
            var _first = Firsts.SingleOrDefault(x => x.NonTerminal == term.Caller);

            foreach (var terminal in first.Terminals)
                _first.AddTerminal(terminal);

            return _first;
        }

        private First AddFirst(Term term, string terminal)
        {
            var first = Firsts.Single(x => x.NonTerminal == term.Caller);
            first.AddTerminal(new Terminal(terminal));

            return first;
        }

        private void InitializeFirst(Term term)
        {
            if (Firsts.All(x => x.NonTerminal != term.Caller))
                Firsts.Add(new First(term.Caller, new List<Terminal>()));
        }
    }

    public class First
    {
        public NonTerminal NonTerminal { get; }
        public ICollection<Terminal> Terminals { get; private set; }

        public First(NonTerminal nonTerminal, ICollection<Terminal> terminals)
        {
            NonTerminal = nonTerminal;
            Terminals = terminals;
        }

        public override string ToString() => $"first({NonTerminal}) => [{string.Join(", ", Terminals)}]";

        public void AddTerminal(Terminal terminal)
        {
            if (!Terminals.Select(x => x.Value).Contains(terminal.Value))
                Terminals.Add(terminal);
        }
    }

    public class Terminal
    {
        public string Value { get; }
        public Terminal(char value) => Value = value.ToString();
        public Terminal(string value) => Value = value;
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
        public string[] Derivations { get; }

        public Term(NonTerminal caller, string called)
        {
            Caller = caller;
            Derivations = called.Split("|");
        }

        public override string ToString() => $"{Caller} -> {string.Join(" | ", Derivations)}";

    }

    public static class NonTerminalExtention
    {
        public static NonTerminal ToNonTerminal(this char value) => new NonTerminal(value);
    }
}
