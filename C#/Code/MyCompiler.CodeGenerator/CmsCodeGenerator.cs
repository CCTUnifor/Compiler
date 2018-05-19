using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CCTUnifor.Logger;
using MyCompiler.CodeGenerator.Aspects;
using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.CodeGenerator.StatmentHandlers.CMS;
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
        public static int Milliseconds = DateTime.Now.Millisecond;

        private readonly TopDownParser _parser;
        public readonly string _input;
        public ICollection<CmsCode> Codes { get; set; }
        public TopDownTokenization Tokenization { get; private set; }
        public Dictionary<string, CmsCode> VariableArea { get; set; }
        private CmsCode StopReference { get; set; }
        public Token Token { get; set; }
        public TinyCodeGeneratorState GeneratorState { get; set; }
        public string CodeGenerated { get; set; }


        public Stack<CmsCode> Stack { get; set; }
        public Stack<Token> TokenStack { get; set; }
        public Stack<Token> AttributionTokenStack { get; set; }
        public Stack<CmsCodeReference> JFCodeReferenceStack { get; set; }
        public Stack<CmsCodeReference> StartWhileCodeReference { get; set; }
        public Stack<CmsCode> RepeatReferenceStack { get; set; }

        public CmsCodeGenerator(TopDownParser parser, string input)
        {
            _parser = parser;
            _input = input;
            Codes = new List<CmsCode>();
            Tokenization = new TopDownTokenization(_parser.NonTerminals, input);
            VariableArea = new Dictionary<string, CmsCode>();
            StopReference = new CmsCode(0X0000);
            TokenStack = new Stack<Token>();
            AttributionTokenStack = new Stack<Token>();
            JFCodeReferenceStack = new Stack<CmsCodeReference>();
            StartWhileCodeReference = new Stack<CmsCodeReference>();
            RepeatReferenceStack = new Stack<CmsCode>();
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
            TokenStack.Push(Token);

            AddCode(CmsCodeFactory.LSP(new CmsCode(0X0010)));
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
            GeneratorState = TinyCodeGeneratorState.Initial;
            while (Token != null)
            {
                IStatmentHandler statmentHandler;
                switch (GeneratorState)
                {
                    case TinyCodeGeneratorState.Initial:
                        statmentHandler = new InitialStatmentHandler();
                        break;
                    case TinyCodeGeneratorState.Read:
                        statmentHandler = new ReadStatmentHandler();
                        break;
                    case TinyCodeGeneratorState.Write:
                        statmentHandler = new WriteStatmentHandler();
                        break;
                    case TinyCodeGeneratorState.If:
                        statmentHandler = new IfStatmentHandler();
                        break;
                    case TinyCodeGeneratorState.End:
                        statmentHandler = new EndStatmentHandler();
                        break;
                    case TinyCodeGeneratorState.While:
                        statmentHandler = new WhileStatmentHandler();
                        break;
                    case TinyCodeGeneratorState.Attribution:
                        statmentHandler = new AttributionStatmentHandler();
                        break;
                    case TinyCodeGeneratorState.Repeat:
                        statmentHandler = new RepeatStatmentHandler();
                        break;
                    case TinyCodeGeneratorState.Until:
                        statmentHandler = new UntilStatmentHandler();
                        break;
                    default:
                        statmentHandler = new InitialStatmentHandler();
                        break;
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
            Logger.PrintLn("");
            var length = 0;
            foreach (var code in Codes)
            {
                Logger.PrintLn($"Byte[{length.ToHexadecimal()}] - {code}");
                length += code.Length;
            }
        }

        public void Export()
        {
            var file = $"fonte";
            var path = $"{file}.OBJ";

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var selectMany = Codes.SelectMany(x => x.Bytes).ToArray();
                foreach (var code in selectMany)
                    fs.WriteByte(code);
                //fs.WriteByte("FF".ToConvertByte()[0]);
            }

            CodeGenerated = file;
        }

        public void ExecuteVM()
        {
            Logger.PrintLn("\n\nExecuting CMS VM");
            Logger.PrintLn($"File -> '{CodeGenerated}'");

            var info = new ProcessStartInfo()
            {
                FileName = "Logs/_cms.exe"
            };
            Logger.IsToPrintInConsole = false;

            try
            {
                using (var exe = Process.Start(info))
                {
                    //using (var output = exe.StandardOutput)
                    //{
                    //    var line = output.ReadLine();
                    //    while (line != null)
                    //        Logger.PrintLn(line);
                    //}
                    exe.WaitForExit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Logger.IsToPrintInConsole = false;
            Console.ReadLine();
        }

        public void MoveNextToken() => Token = Tokenization.GetTokenIgnoreSpace();
        public void RemoveParentheses<T>() where T : Token
        {
            while (!(Token is T) && Token != null)
                MoveNextToken();
            if (Token is T)
                MoveNextToken();
        }
    }
}
