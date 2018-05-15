using System;
using System.Collections.Generic;
using System.Text;
using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar.Tokens;
using MyCompiler.Grammar.Tokens.Terminals;
using MyCompiler.Tokenization.TopDown;

namespace MyCompiler.CodeGenerator.StatmentHandlers
{
    public class ExpressionStatmentHandler : IStatmentHandler
    {
        private CmsCodeExpressionStatmentState ExpressionStatmentState { get; set; }
        public CmsCode InitialCode { get; set; }

        public void Handler(CmsCodeGenerator generator)
        {
            CmsCode compReference = null;
            ExpressionStatmentState = CmsCodeExpressionStatmentState.Initial;
            while (!(generator.Token is ThenToken || generator.Token is DoToken || generator.Token is SemiColonToken))
            {
                switch (ExpressionStatmentState)
                {
                    case CmsCodeExpressionStatmentState.Initial:
                        HandleInitial(generator);
                        break;
                    case CmsCodeExpressionStatmentState.Adress:
                        GenerateInitialCode(generator);
                        generator.AddCode(CmsCodeFactory.LOD(generator.VariableArea[generator.Token.Value]));
                        if (compReference != null)
                            generator.AddCode(compReference);
                        GoToInitialIfState(generator);
                        break;
                    case CmsCodeExpressionStatmentState.Number:
                        GenerateInitialCode(generator);
                        generator.AddCode(CmsCodeFactory.LDI(new CmsCode(int.Parse(generator.Token.Value))));
                        if (compReference != null)
                            generator.AddCode(compReference);
                        GoToInitialIfState(generator);
                        break;
                    case CmsCodeExpressionStatmentState.GreatThen:
                        compReference = CmsCodeFactory.GT;
                        GoToInitialIfState(generator);
                        break;
                    case CmsCodeExpressionStatmentState.LessThen:
                        compReference = CmsCodeFactory.LT;
                        GoToInitialIfState(generator);
                        break;
                    case CmsCodeExpressionStatmentState.EqualsThen:
                        compReference = CmsCodeFactory.EQ;
                        GoToInitialIfState(generator);
                        break;
                    case CmsCodeExpressionStatmentState.GreatEqualThen:
                        compReference = CmsCodeFactory.GE;
                        GoToInitialIfState(generator);
                        break;
                    case CmsCodeExpressionStatmentState.LessEqualThen:
                        compReference = CmsCodeFactory.LE;
                        GoToInitialIfState(generator);
                        break;
                    case CmsCodeExpressionStatmentState.NotEqualsThen:
                        compReference = CmsCodeFactory.NE;
                        GoToInitialIfState(generator);
                        break;
                    case CmsCodeExpressionStatmentState.Plus:
                        generator.Token = generator.Tokenization.GetTokenIgnoreSpace();
                        generator.AddCode(CmsCodeFactory.ADI(new CmsCode(int.Parse(generator.Token.Value))));
                        GoToInitialIfState(generator);
                        break;
                    case CmsCodeExpressionStatmentState.Sub:
                        compReference = CmsCodeFactory.SUI;
                        GoToInitialIfState(generator);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void GenerateInitialCode(CmsCodeGenerator generator)
        {
            if (InitialCode == null)
                InitialCode = new CmsCode(generator.CodesLengh);
        }

        private void HandleInitial(CmsCodeGenerator generator)
        {
            if (generator.Token.IsIdentifier())
                ExpressionStatmentState = CmsCodeExpressionStatmentState.Adress;
            else if (generator.Token.IsNumber())
                ExpressionStatmentState = CmsCodeExpressionStatmentState.Number;
            else if (generator.Token is EqualToken)
                ExpressionStatmentState = CmsCodeExpressionStatmentState.EqualsThen;
            else if (generator.Token is NotEqualToken)
                ExpressionStatmentState = CmsCodeExpressionStatmentState.NotEqualsThen;
            else if (generator.Token is GreatToken)
                ExpressionStatmentState = CmsCodeExpressionStatmentState.GreatThen;
            else if (generator.Token is LessToken)
                ExpressionStatmentState = CmsCodeExpressionStatmentState.LessThen;
            else if (generator.Token is GreatOrEqualToken)
                ExpressionStatmentState = CmsCodeExpressionStatmentState.GreatEqualThen;
            else if (generator.Token is LessOrEqualToken)
                ExpressionStatmentState = CmsCodeExpressionStatmentState.LessEqualThen;
            else if (generator.Token is PlusToken)
                ExpressionStatmentState = CmsCodeExpressionStatmentState.Plus;
            else if (generator.Token is SubToken)
                ExpressionStatmentState = CmsCodeExpressionStatmentState.Sub;
            //else if (generator.Token is Token)
            //    ExpressionStatmentState = CmsCodeExpressionStatmentState.Plus;
            else
                generator.Token = generator.Tokenization.GetTokenIgnoreSpace();
        }

        private void GoToInitialIfState(CmsCodeGenerator generator)
        {
            generator.Token = generator.Tokenization.GetTokenIgnoreSpace();
            ExpressionStatmentState = CmsCodeExpressionStatmentState.Initial;
        }
    }
}
