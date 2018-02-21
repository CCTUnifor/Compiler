using System;
using System.Collections.Generic;
using System.Linq;
using MyCompiler.Core.Interfaces;

namespace MyCompiler.Core.Models
{
    public class GenericLexicalAnalyzer : ILexicalAnalyzer
    {
        private readonly string[] _regularExpresions;
        private readonly IThompsonConstruction thompsonConstruction;
        public Terminal LastTerminal { get; set; }
        public ThompsonEnum State { get; set; }

        public GenericLexicalAnalyzer(string[] regularExpresions)
        {
            _regularExpresions = regularExpresions;
            thompsonConstruction = new ThompsonConstruction();
            State = ThompsonEnum.Initial;
        }

        public IEnumerable<Token> LoadTokens(string input)
        {
            input = input.Replace(" ", "");
            NewMethod(input, 0);

            return null;
        }

        private void NewMethod(string input, int i)
        {
            Vertex initialNode = null;//= new Vertex>();

            while (i < input.Length)
            {
                var value = new Terminal(input[i]);

                switch (State)
                {
                    case ThompsonEnum.Initial:
                        i = SetInitialState(i, value);
                        break;
                    case ThompsonEnum.Terminal:
                        if (LastTerminal == null)
                        {
                            LastTerminal = value;
                            initialNode = value.SimpleStatment();
                        }
                        else if (LastTerminal.IsTerminal)
                        {
                            initialNode = thompsonConstruction.ConcatStatment(LastTerminal, value);
                        }

                        break;
                    case ThompsonEnum.Final:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                i++;
            }

            Print(initialNode);
        }

        private void Print(Vertex initialNode)
        {
            foreach (var edge in initialNode.Edges)
            {
                Console.WriteLine(edge);
                if (edge.End.Edges.Any())
                    Print(edge.End);
            }
            //throw new NotImplementedException();
        }

        private int SetInitialState(int i, Terminal value)
        {
            if (value.IsTerminal)
            {
                State = ThompsonEnum.Terminal;
                i--;
            }

            return i;
        }

        private static void ThompsonConstruction(string input)
        {
            int i = 0;
            IThompsonConstruction x = new ThompsonConstruction();
            var terminalA = new Terminal('a');
            var terminalB = new Terminal('b');

            x.ConcatStatment(terminalA, terminalB);

        }
    }

    public enum ThompsonEnum
    {
        Final = -1,
        Initial = 1,
        Terminal = 2,
    }
}