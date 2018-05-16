using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        public static int Milliseconds = DateTime.Now.Millisecond;

        private readonly TopDownParser _parser;
        public readonly string _input;
        public ICollection<CmsCode> Codes { get; set; }
        public TopDownTokenization Tokenization { get; private set; }
        public Dictionary<string, CmsCode> VariableArea { get; set; }
        private CmsCode StopReference { get; set; }
        public Token Token { get; set; }
        public CmsCodeState State { get; set; }
        public string CodeGenerated { get; set; }


        public Stack<CmsCode> Stack { get; set; }
        public Stack<Token> TokenStack { get; set; }
        public Stack<Token> AttributionTokenStack { get; set; }
        public Stack<CmsCodeReference> JFCodeReferenceStack { get; set; }
        public Stack<CmsCodeReference> StartWhileCodeReference { get; set; }

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
            State = CmsCodeState.Initial;
            while (Token != null)
            {
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
            var file = $"export{Milliseconds}";
            var path = $"Logs/{file}.OBJ";

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var selectMany = Codes.SelectMany(x => x.Bytes).ToArray();
                foreach (var code in selectMany)
                    fs.WriteByte(code);
                fs.WriteByte("FF".ToConvertByte()[0]);
            }

            CodeGenerated = file;
        }

        public void ExecuteVM()
        {
            Logger.PrintLn("\n\nExecuting CMS VM");
            Logger.PrintLn($"File -> '{CodeGenerated}'");

            // java - jar CmsJava.jar

            var info = new ProcessStartInfo()
            {
                FileName = "Logs/cms.exe",
                //FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };


            try
            {
                using (var exe = Process.Start(info))
                {
                    using (var input = exe.StandardInput)
                    {
                        //input.WriteLine($"java -jar CmsJava.jar Logs/{CodeGenerated}");
                        input.WriteLine($"{CodeGenerated}");
                        //PrintProcess(exe);

                        //Task.Delay(1000).Wait();
                        //PrintProcess(exe);

                        foreach (ProcessThread thread in exe.Threads)
                            if (thread.ThreadState == ThreadState.Wait
                                && thread.WaitReason == ThreadWaitReason.UserRequest)
                                NewMethod(exe, input);
                    }

                    using (var error = exe.StandardError)
                        Logger.PrintLn(error.ReadLine());

                    PrintProcess(exe);

                    exe.WaitForExit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            Console.ReadLine();
        }

        private static void NewMethod(Process exe, StreamWriter input)
        {
            input.WriteLine(Console.ReadLine());
        }

        private static void PrintProcess(Process exe)
        {
            using (var output = exe.StandardOutput)
            {
                string line = output.ReadLine();
                while (line != null)
                {
                    if (string.IsNullOrEmpty(line) && !line.Contains("Microsoft") && !line.Contains("java -jar") && !line.Contains("File:"))
                        Logger.PrintLn(line);
                    line = output.ReadLine();
                }
            }
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
