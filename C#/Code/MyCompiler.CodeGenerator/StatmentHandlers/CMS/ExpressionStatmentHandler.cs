using System;
using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Code.Factories;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar.Tokens;
using MyCompiler.Grammar.Tokens.Terminals;
using MyCompiler.Tokenization.TopDown;

namespace MyCompiler.CodeGenerator.StatmentHandlers.CMS
{
    public class ExpressionStatmentHandler : IStatmentHandler
    {
        private CmsCodeExpressionStatmentState ExpressionStatmentState { get; set; }

        public void Handler(CmsCodeGenerator generator)
        {
            try
            {
                CmsCode compReference = null;
                ExpressionStatmentState = CmsCodeExpressionStatmentState.Initial;
                generator.MoveNextToken();

                while (!(generator.Token is ThenToken || generator.Token is DoToken || generator.Token is SemiColonToken || generator.Token is EndToken))
                {
                    switch (ExpressionStatmentState)
                    {
                        case CmsCodeExpressionStatmentState.Initial:
                            HandleInitial(generator);
                            break;
                        case CmsCodeExpressionStatmentState.Adress:
                            generator.AddCode(CmsCodeFactory.LOD(generator.VariableArea[generator.Token.Value]));
                            if (compReference != null)
                                generator.AddCode(compReference);
                            GoToInitialIfState(generator);
                            break;
                        case CmsCodeExpressionStatmentState.Number:
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
                            generator.MoveNextToken();
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
            catch (Exception e)
            {
                throw new Exception($"Token: {generator.Token}\nTokenStack: {generator.Stack}\nError: {e.Message}", e);
            }
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
                generator.MoveNextToken();
        }

        private void GoToInitialIfState(CmsCodeGenerator generator)
        {
            generator.MoveNextToken();
            ExpressionStatmentState = CmsCodeExpressionStatmentState.Initial;
        }
    }
}
