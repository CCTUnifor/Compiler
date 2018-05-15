using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CCTUnifor.Logger;
using MyCompiler.CodeGenerator.Aspects;
using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.CodeGenerator.StatmentHandlers;
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

        public Stack<Token> TokenStack { get; set; }
        public Stack<CmsCodeReference> JFCode { get; set; }
        public Stack<CmsCodeReference> InitialWhileCode { get; set; }

        public CmsCodeGenerator(TopDownParser parser, string input)
        {
            _parser = parser;
            _input = input;
            Codes = new List<CmsCode>();
            Tokenization = new TopDownTokenization(_parser.NonTerminals, input);
            VariableArea = new Dictionary<string, CmsCode>();
            StopReference = new CmsCode(0X0000);
            TokenStack = new Stack<Token>();
            JFCode = new Stack<CmsCodeReference>();
            InitialWhileCode = new Stack<CmsCodeReference>();
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

            HandlerBody();
        }

        private void End() => AddCode(CmsCodeFactory.STOP);

        private void HandlerBody()
        {
            State = CmsCodeState.Initial;
            while (Token != null)
            {
                Token = Tokenization.GetTokenIgnoreSpace();
                if (Token == null) continue;

                IStatmentHandler statmentHandler;
                switch (State)
                {
                    case CmsCodeState.Initial:
                        statmentHandler = new InitialStatmentHandler();
                        break;
                    case CmsCodeState.Read:
                        statmentHandler = new ReadStatmentHandler();
                        break;
                    case CmsCodeState.Write:
                        statmentHandler = new WriteStatmentHandler();
                        break;
                    case CmsCodeState.If:
                        statmentHandler = new IfStatmentHandler();
                        break;
                    case CmsCodeState.End:
                        statmentHandler = new EndStatmentHandler();
                        break;
                    case CmsCodeState.While:
                        statmentHandler = new WhileStatmentHandler();
                        break;
                    case CmsCodeState.Attribution:
                        statmentHandler = new AttributionStatmentHandler();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                statmentHandler.Handler(this);
            }
        }

        [AddCodeAspect]
        public void AddCode(CmsCode code)
        {
            Codes.Add(code);
            IncrementStopReference();
        }

        private void IncrementStopReference() => StopReference.ValueDecimal = CodesLengh;
        public int CodesLengh => Codes.Sum(x => x.Length);

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

        public void Export()
        {
            var path = $"Logs/export-{DateTime.Now.Millisecond}";
            Check(path);

            var byteArray = Codes.SelectMany(x => Convert.ToByte(x.Export())).ToArray();

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
            }
        }

        private static void Check(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
    }
}
