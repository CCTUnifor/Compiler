using System;
using MyCompiler.Core.Enums;
using MyCompiler.Core.Exceptions;
using MyCompiler.Core.Models.LexicalAnalyzes;
using MyCompiler.Core.Models.Tokens;

namespace MyCompiler.Core.Models.SyntacticAnalyzes
{
    public class TinySyntacticAnalyzer
    {
        public TinyLexicalAnalyze LexicalAnalyze { get; set; }

        public TinyToken Peek => LexicalAnalyze.LastToken;
        private bool HasNext() => LexicalAnalyze.Any();
        private int Line { get; set; }

        private void Eat(string c)
        {
            if (Peek == null)
                throw new ExpectedException(c, "null");
            if (c.ToString().ToLower() != Peek.Value.ToLower()) // TODO
                throw new ExpectedException(c.ToString(), Peek.Value);

            Console.WriteLine($"++++++ EAT - {c} ++++++");
            LexicalAnalyze.GetNextToken();
        }

        private void Eat(TinyGrammar c)
        {
            if (Peek == null)
                throw new ExpectedException("c", "null");
            if (c != Peek.Grammar)
                throw new ExpectedException(c.ToString(), Peek.Grammar.ToString());

            Console.WriteLine($"++++++ EAT - {c} ++++++");
            LexicalAnalyze.GetNextToken();
        }

        public void Check(int countLine, string input)
        {
            Line = countLine;
            LexicalAnalyze = new TinyLexicalAnalyze(countLine, input);
            DeclSequencia();
        }

        private void DeclSequencia()
        {
            Declaration();
            if (!HasNext() || Peek.Grammar != TinyGrammar.SemiColon) return;

            Eat(";");
            DeclSequencia();
        }

        private void Declaration()
        {
            switch (Peek.Grammar)
            {
                case TinyGrammar.ReserveWord:
                    switch (Peek.Value)
                    {
                        case "read":
                            ReadDecl();
                            break;
                        case "write":
                            WriteDecl();
                            break;
                        case "if":
                            CondDecl();
                            break;
                        case "repeat":
                            RepetDecl();
                            break;
                        default:
                            throw new ExpectedException("ReserveWord", Peek.Value);
                    }

                    break;
                case TinyGrammar.Identifier:
                    AtribDecl();
                    break;
                default:
                    throw new CompilationException($"in line :{Line} - <declaration>");
            }
        }

        private void ReadDecl()
        {
            Eat("write");
            Exp();
        }

        private void WriteDecl()
        {
            ExpSimple();
            if (!HasNext() || Peek.Grammar != TinyGrammar.Sum) return;

            CompOp();
            ExpSimple();
        }

        private void Exp()
        {
            ExpSimple();
            if (!HasNext() || Peek.Grammar != TinyGrammar.Sum) return;

            CompOp();
            ExpSimple();
        }

        private void CompOp()
        {
            if (Peek.Value == "<")
                Eat("<");
            else
                Eat("=");
        }

        private void ExpSimple()
        {
            Term();
            if (HasNext() && Peek.Grammar == TinyGrammar.Sum)
                ExpSimpleLine();
        }

        private void ExpSimpleLine()
        {
            Sum();
            Term();
            if (Peek.Grammar == TinyGrammar.Sum)
                ExpSimpleLine();
        }

        private void Sum()
        {
            if (Peek.Value == "+")
                Eat("+");
            else 
                Eat("-");
        }

        private void Term()
        {
            Factor();
            if (HasNext() && Peek.Grammar == TinyGrammar.Prod)
                TermLine();
        }

        private void Factor()
        {
            if (Peek.Grammar == TinyGrammar.Parentheses)
            {
                Eat("(");
                Exp();
                Eat(")");
            }
            else if (Peek.Grammar == TinyGrammar.Number)
                Eat(TinyGrammar.Number);
            else if (Peek.Grammar == TinyGrammar.Identifier)
                Eat(TinyGrammar.Identifier);
            else
                throw new CompilationException($"in line :{Line} - <factor>");

        }

        private void TermLine()
        {
            Mult();
            Factor();
            if (Peek.Grammar == TinyGrammar.Prod)
                TermLine();
        }

        private void Mult()
        {
            if (Peek.Value == "*")
                Eat("*");
            else 
                Eat("/");
        }

        private void CondDecl()
        {
            Eat("if");
            Exp();
            Eat("then");
            DeclSequencia();

            if (HasNext() && Peek.Value == "else")
            {
                Eat("else");
                DeclSequencia();
            }

            Eat("end");
        }

        private void RepetDecl()
        {
            Eat("repeat");
            DeclSequencia();
            Eat("until");
            Exp();
        }

        private void AtribDecl()
        {
            Eat(TinyGrammar.Identifier);
            Eat(TinyGrammar.Attribution);
            Exp();
        }
    }
}