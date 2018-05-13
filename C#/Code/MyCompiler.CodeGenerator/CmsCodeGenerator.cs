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

        public CmsCodeGenerator(TopDownParser parser, string input)
        {
            _parser = parser;
            _input = input;
            Codes = new List<CmsCode>();
            Tokenization = new TopDownTokenization(_parser.NonTerminals, input);
            VariableArea = new Dictionary<string, CmsCode>();
            StopReference = new CmsCode(0X0000);
        }

        public void Generator()
        {
            Header();
            Body();
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
                        AddCode(CmsCodeFactory.IN);
                        AddCode(CmsCodeFactory.STO(VariableArea[Token.Value]));
                        State = CmsCodeState.Initial;
                        break;
                    case CmsCodeState.Write:
                        AddCode(CmsCodeFactory.LOD(VariableArea[Token.Value]));
                        AddCode(CmsCodeFactory.OUT);
                        State = CmsCodeState.Initial;
                        break;
                    case CmsCodeState.If:
                        Token = Tokenization.GetTokenIgnoreSpace();
                        AdressOrHardNumber();

                        CmsCode compReference = null;
                        Token = Tokenization.GetTokenIgnoreSpace();
                        if (Token.Value == ">")
                            compReference = CmsCodeFactory.GT;

                        Token = Tokenization.GetTokenIgnoreSpace();
                        AdressOrHardNumber();
                        AddCode(compReference);

                        var jfReference = new CmsCode(CodesLengh);
                        var jf = CmsCodeFactory.JF(jfReference);
                        AddCode(jf);

                        // body IF



                        break;
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
                    break;
            }
        }

        [AddCodeAspect]
        private void AddCode(CmsCode code)
        {
            Codes.Add(code);
            IncrementStopReference();
        }

        private void AdressOrHardNumber()
        {
            if (Token.IsIdentifier())
                AddCode(CmsCodeFactory.LOD(VariableArea[Token.Value]));
            else if (Token.IsNumber())
                AddCode(CmsCodeFactory.LDI(new CmsCode(int.Parse(Token.Value))));
        }

        private void IncrementStopReference() => StopReference.ValueDecimal = CodesLengh + 2;
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
        If
    }
}
