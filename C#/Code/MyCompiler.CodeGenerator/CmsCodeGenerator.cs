using System;
using System.Collections.Generic;
using MyCompiler.CodeGenerator.Aspects;
using MyCompiler.CodeGenerator.Code;
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

        public CmsCodeGenerator(TopDownParser parser, string input)
        {
            _parser = parser;
            _input = input;
            Codes = new List<CmsCode>();
        }

        public void Generator()
        {
            Header();
        }

        private void Header()
        {
            AddCode(CmsCodeFactory.LSP(new CmsCode(0X0010)));
            GenerateVariableArea(_input);
        }

        private void GenerateVariableArea(string input, VariableAreaState variableAreaState = VariableAreaState.Initial)
        {
            var tokenize = new TopDownTokenization(_parser.NonTerminals, input);
            //switch (variableAreaState)
            //{
            //    case VariableAreaState.Initial:

            //        break;

            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}

            AddCode(CmsCodeFactory.JMP(new CmsCode(0X0800)));
        }

        [AddCodeAspect]
        private void AddCode(CmsCode code) => Codes.Add(code);
    }

    public enum VariableAreaState
    {
        Initial
    }
}
