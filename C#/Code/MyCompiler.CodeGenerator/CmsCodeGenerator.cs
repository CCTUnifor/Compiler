using System;
using System.Collections.Generic;
using System.Linq;
using CCTUnifor.Logger;
using MyCompiler.CodeGenerator.Aspects;
using MyCompiler.CodeGenerator.Code;
using MyCompiler.Core.Exceptions;
using MyCompiler.Core.Extensions;
using MyCompiler.Core.Interfaces;
using MyCompiler.Grammar;
using MyCompiler.Parser;
using MyCompiler.Tokenization.TopDown;

namespace MyCompiler.CodeGenerator
{
    public class CmsCodeGenerator : ICodeGenerator
    {
        private readonly TopDownParser _parser;
        private readonly string _input;
        public ICollection<CmsCode> Codes { get; set; }
        public TopDownTokenization Tokenization { get; private set; }
        public Dictionary<string, CmsCode> VariableArea { get; set; }
        private CmsCode StopReference { get; set; }
        public Stack<CmsCode> Stack { get; set; }
        public Token Token { get; set; }
        public CmsCodeState State { get; set; }
        public CmsCodeIfStatmentState IfStatmentState { get; set; }
        public Stack<Token> TokenStack { get; set; }
        public CmsCodeReference JFCode { get; set; }

        public CmsCodeGenerator(TopDownParser parser, string input)
        {
            _parser = parser;
            _input = input;
            Codes = new List<CmsCode>();
            Tokenization = new TopDownTokenization(_parser.NonTerminals, input);
            VariableArea = new Dictionary<string, CmsCode>();
            StopReference = new CmsCode(0X0000);
            TokenStack = new Stack<Token>();
        }

        public void Generator()
        {
            Header();
            Body();
            End();

            Print();
        }

        private void Header()
        {
            Token = Tokenization.GetToken();
            if (!Token.IsProgram())
                throw new ExpectedException("PROGRAM", Token.Value, null);

            AddCode(CmsCodeFactory.LSP(StopReference));
            GenerateVariableArea();
        }

        private void GenerateVariableArea()
        {
            Token = Tokenization.GetTokenIgnoreSpace();
            var jmpReference = new CmsCode(0X00);
            var jmp = CmsCodeFactory.JMP(jmpReference);

            AddCode(jmp);

            while (Token != null && !Token.IsBegin())
            {
                if (Token.IsVar())
                {
                    Token = Tokenization.GetTokenIgnoreSpace();
                    VariableArea.Add(Token.Value, new CmsCode(CodesLengh));
                    Malock();
                }

                Token = Tokenization.GetTokenIgnoreSpace();
            }

            jmpReference.ValueDecimal = CodesLengh;
        }

        private void Body()
        {
            if (!Token.IsBegin())
                throw new ExpectedException("BEGIN", Token.Value, null);

            State = CmsCodeState.Initial;
            HandleBody();
        }

        private void End() => AddCode(CmsCodeFactory.STOP);

        private void HandleBody()
        {
            while (Token != null)
            {
                Token = Tokenization.GetTokenIgnoreSpace();
                if (Token == null) continue;

                switch (State)
                {
                    case CmsCodeState.Initial:
                        HandleInitialState();
                        break;
                    case CmsCodeState.Read:
                        HandleReadStatment();
                        break;
                    case CmsCodeState.Write:
                        HandleWriteStatment();
                        break;
                    case CmsCodeState.If:
                        IfStatmentState = CmsCodeIfStatmentState.Initial;
                        HandleIfStatment();
                        break;
                    case CmsCodeState.End:
                        HandleEndStatment();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void HandleInitialState()
        {
            switch (Token.Value.ToLower())
            {
                case "read":
                    State = CmsCodeState.Read;
                    break;
                case "write":
                    State = CmsCodeState.Write;
                    break;
                case "if":
                    State = CmsCodeState.If;
                    TokenStack.Push(Token);
                    break;
                case "end":
                    State = CmsCodeState.End;
                    break;
            }
        }

        private void HandleReadStatment()
        {
            AddCode(CmsCodeFactory.IN);
            AddCode(CmsCodeFactory.STO(VariableArea[Token.Value]));
            State = CmsCodeState.Initial;
        }

        private void HandleWriteStatment()
        {
            AddCode(CmsCodeFactory.LOD(VariableArea[Token.Value]));
            AddCode(CmsCodeFactory.OUT);
            State = CmsCodeState.Initial;
        }

        private void HandleIfStatment()
        {
            CmsCode compReference = null;
            while (Token.Value.ToLower() != "then")
            {
                switch (IfStatmentState)
                {
                    case CmsCodeIfStatmentState.Initial:
                        if (Token.IsIdentifier())
                            IfStatmentState = CmsCodeIfStatmentState.Adress;
                        else if (Token.IsNumber())
                            IfStatmentState = CmsCodeIfStatmentState.Number;
                        else if (Token.Value == ">")
                            IfStatmentState = CmsCodeIfStatmentState.GreatThen;
                        else
                            Token = Tokenization.GetTokenIgnoreSpace();
                        break;
                    case CmsCodeIfStatmentState.Adress:
                        AddCode(CmsCodeFactory.LOD(VariableArea[Token.Value]));
                        if (compReference != null)
                            AddCode(compReference);
                        GoToInitialIfState();
                        break;
                    case CmsCodeIfStatmentState.Number:
                        AddCode(CmsCodeFactory.LDI(new CmsCode(int.Parse(Token.Value))));
                        if (compReference != null)
                            AddCode(compReference);
                        GoToInitialIfState();
                        break;
                    case CmsCodeIfStatmentState.GreatThen:
                        compReference = CmsCodeFactory.GT;
                        GoToInitialIfState();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            JFCode = (CmsCodeReference) CmsCodeFactory.JF(new CmsCode(CodesLengh));
            AddCode(JFCode);

            State = CmsCodeState.Initial;
        }

        private void HandleEndStatment()
        {
            var pop = TokenStack.Pop();
            switch (pop.Value.ToLower())
            {
                case "if":
                    JFCode.Reference.ValueDecimal = CodesLengh;
                    break;
            }

            State = CmsCodeState.Initial;
        }

        [AddCodeAspect]
        private void AddCode(CmsCode code)
        {
            Codes.Add(code);
            IncrementStopReference();
        }

        private void GoToInitialIfState()
        {
            Token = Tokenization.GetTokenIgnoreSpace();
            IfStatmentState = CmsCodeIfStatmentState.Initial;
        }

        private void IncrementStopReference() => StopReference.ValueDecimal = CodesLengh;
        private int CodesLengh => Codes.Sum(x => x.Length);
        private void Malock() => AddCode(new CmsCode(0X00));
        private void Print()
        {
            var length = 0;
            foreach (var code in Codes)
            {
                Logger.PrintLn($"Byte[{length.ToHexadecimal()}] - {code}");
                length += code.Length;
            }
        }
    }

    public enum CmsCodeState
    {
        Initial,
        Read,
        Write,
        If,
        End
    }

    public enum CmsCodeIfStatmentState
    {
        Initial,
        Adress,
        Number,
        GreatThen
    }
}
