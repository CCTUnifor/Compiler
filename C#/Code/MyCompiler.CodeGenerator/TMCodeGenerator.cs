using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CCTUnifor.Logger;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.CodeGenerator.StatmentHandlers.TM;
using MyCompiler.Core.Interfaces;
using MyCompiler.Grammar;
using MyCompiler.Grammar.Tokens.Terminals;
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
        public int[] Registradores { get; set; }
        public Dictionary<Token, int> VarDictionary { get; set; }

        public TmCodeGenerator(TopDownParser parser, string input)
        {
            _parser = parser;
            _input = input;
            Tokenization = new TopDownTokenization(_parser.NonTerminals, input);
            Instructions = new List<string>();
            Registradores = new int[8];
            VarDictionary = new Dictionary<Token, int>();
        }

        public void Generator()
        {
            Header();
            Body();
            End();
        }

        private void End()
            => Instructions.Add(InstructionFactory.Halt(Instructions.Count));

        private void Header()
        {
            MoveNextToken(); // PROGRAM
            MoveNextToken(); // VAR
            var regCount = 0;
            while (Token != null && Token is VarToken)
            {
                MoveNextToken(); // IDE
                VarDictionary.Add(Token, regCount++);
                MoveNextToken(); // :
                MoveNextToken(); // INTEGER
                MoveNextToken(); // ;
                MoveNextToken();
            }
            MoveNextToken(); // BEGIN

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
                        case TinyCodeGeneratorState.Write:
                            statmentHandler = new WriteStatmentTMHandler();
                            break;
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
                            MoveNextToken();
                            break;
                    }
                    statmentHandler.Handler(this);
                }
            }
        }

        public void Export()
        {
            var file = $"Logs/fonte";
            var path = $"{file}.tm";

            using (var fs = new StreamWriter(path))
            {
                var selectMany = Instructions.ToArray();
                foreach (var instruction in selectMany)
                    fs.WriteLine(instruction);
            }
        }

        public void ExecuteVM()
        {
            Logger.PrintLn("\n\nExecuting CMS VM");
            Logger.PrintLn($"File -> fonte.tm");

            var info = new ProcessStartInfo()
            {
                FileName = "Logs/tm.exe",
                Arguments = "fonte.tm"
            };
            Logger.IsToPrintInConsole = false;

            try
            {
                using (var exe = Process.Start(info))
                    exe.WaitForExit();
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

    internal static class InstructionFactory
    {
        public static string Halt(int line) => $"{line}: {OpCodeTM.HALT} 0,0,0";

        public static string Read(int line, int reg) => $"{line}: {OpCodeTM.IN} {reg},0,0";

        public static string Write(int line, int reg) => $"{line}: {OpCodeTM.OUT} {reg},0,0";
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