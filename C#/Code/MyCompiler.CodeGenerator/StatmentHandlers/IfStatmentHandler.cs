using System;
using System.Collections.Generic;
using System.Text;
using MyCompiler.CodeGenerator.Code;
using MyCompiler.CodeGenerator.Enums;
using MyCompiler.CodeGenerator.Interfaces;
using MyCompiler.Grammar;

namespace MyCompiler.CodeGenerator.StatmentHandlers
{
    public class IfStatmentHandler : IStatmentHandler
    {
        public CmsCodeIfStatmentState IfStatmentState { get; set; }

        public void Handler(CmsCodeGenerator generator)
        {
            CmsCode compReference = null;
            IfStatmentState = CmsCodeIfStatmentState.Initial;
            while (generator.Token.Value.ToLower() != "then")
            {
                switch (IfStatmentState)
                {
                    case CmsCodeIfStatmentState.Initial:
                        if (generator.Token.IsIdentifier())
                            IfStatmentState = CmsCodeIfStatmentState.Adress;
                        else if (generator.Token.IsNumber())
                            IfStatmentState = CmsCodeIfStatmentState.Number;
                        else if (generator.Token.Value == ">")
                            IfStatmentState = CmsCodeIfStatmentState.GreatThen;
                        else
                            generator.Token = generator.Tokenization.GetTokenIgnoreSpace();
                        break;
                    case CmsCodeIfStatmentState.Adress:
                        generator.AddCode(CmsCodeFactory.LOD(generator.VariableArea[generator.Token.Value]));
                        if (compReference != null)
                            generator.AddCode(compReference);
                        GoToInitialIfState(generator);
                        break;
                    case CmsCodeIfStatmentState.Number:
                        generator.AddCode(CmsCodeFactory.LDI(new CmsCode(int.Parse(generator.Token.Value))));
                        if (compReference != null)
                            generator.AddCode(compReference);
                        GoToInitialIfState(generator);
                        break;
                    case CmsCodeIfStatmentState.GreatThen:
                        compReference = CmsCodeFactory.GT;
                        GoToInitialIfState(generator);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            generator.JFCode = (CmsCodeReference)CmsCodeFactory.JF(new CmsCode(generator.CodesLengh));
            generator.AddCode(generator.JFCode);

            generator.State = CmsCodeState.Initial;
        }

        private void GoToInitialIfState(CmsCodeGenerator generator)
        {
            generator.Token = generator.Tokenization.GetTokenIgnoreSpace();
            IfStatmentState = CmsCodeIfStatmentState.Initial;
        }
    }
}