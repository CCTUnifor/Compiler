using System;
using System.Collections.Generic;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.CodeGenerator.StatmentHandlers.TM;
using MyCompiler.Core.Interfaces;
using MyCompiler.Grammar;
using MyCompiler.Parser;
using MyCompiler.Tokenization.TopDown;

namespace MyCompiler.CodeGenerator
{
    public class TmCodeGenerator : ICodeGenerator
    {
        private readonly TopDownParser _parser;
        private readonly string _input;

        public ICollection<string> Instructions { get; set; }
        public TopDownTokenization Tokenization { get; set; }
        public Token Token { get; set; }
        public TinyCodeGeneratorState GeneratorState { get; set; }

        public TmCodeGenerator(TopDownParser parser, string input)
        {
            _parser = parser;
            _input = input;
            Tokenization = new TopDownTokenization(_parser.NonTerminals, input);
            Instructions = new List<string>();
        }

        public void Generator()
        {
            Header();
            Body();
        }

        private void Header()
        {
            throw new NotImplementedException();
        }

        private void Body()
        {
            while (Token != null)
            {
                GeneratorState = TinyCodeGeneratorState.Initial;
                while (Token != null)
                {
                    IStatmentTMHandler statmentHandler;
                    switch (GeneratorState)
                    {
                        case TinyCodeGeneratorState.Initial:
                            statmentHandler = new InitialStatmentTMHandler();
                            break;
                        case TinyCodeGeneratorState.Read:
                            statmentHandler = new ReadStatmentTMHandler();
                            break;
                        //case TinyCodeGeneratorState.Write:
                        //    statmentHandler = new WriteStatmentHandler();
                        //    break;
                        //case TinyCodeGeneratorState.If:
                        //    statmentHandler = new IfStatmentHandler();
                        //    break;
                        //case TinyCodeGeneratorState.End:
                        //    statmentHandler = new EndStatmentHandler();
                        //    break;
                        //case TinyCodeGeneratorState.While:
                        //    statmentHandler = new WhileStatmentHandler();
                        //    break;
                        //case TinyCodeGeneratorState.Attribution:
                        //    statmentHandler = new AttributionStatmentHandler();
                        //    break;
                        //case TinyCodeGeneratorState.Repeat:
                        //    statmentHandler = new RepeatStatmentHandler();
                        //    break;
                        //case TinyCodeGeneratorState.Until:
                        //    statmentHandler = new UntilStatmentHandler();
                        //    break;
                        default:
                            statmentHandler = new InitialStatmentTMHandler();
                            break;
                    }
                    statmentHandler.Handler(this);
                }
            }
        }

        public void Export()
        {
            throw new NotImplementedException();
        }

        public void ExecuteVM()
        {
            throw new NotImplementedException();
        }

        public void MoveNextToken() => Token = Tokenization.GetTokenIgnoreSpace();
    }

    public enum OpCodeTM
    {
        HALT,
        IN,
        OUT,
        ADD,
        SUB,
        MUL,
        DIV,

        LD,
        LDA,
        LDC,
        ST,
        JLT,

        JLE,
        JGE,
        JGT,
        JEQ,
        JNE
    }
}