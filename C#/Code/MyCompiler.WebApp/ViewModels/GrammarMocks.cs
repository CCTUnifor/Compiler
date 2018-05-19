using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompiler.WebApp.ViewModels
{
    public static class GrammarMocks
    {
        public static string Grammar1 => "PROGRAMA -> PROGRAM VARS-DECL BEGIN DECL-SEQUENCIA END\n" +
                                         "VARS-DECL -> VAR ide : INTEGER ; VARS-DECL | ε\n" +
                                         "DECL-SEQUENCIA -> DECLARACAO DECL-SEQUENCIA'\n" +
                                         "DECL-SEQUENCIA'-> ; DECLARACAO DECL-SEQUENCIA'| ε\n" +
                                         "DECLARACAO -> COND-STATMENT | REPEAT-STATMENT | WHILE-STATMENT | ATRIB-STATMENT | READ-STATMENT | WRITE-STATMENT \n" +
                                         "COND-STATMENT -> IF EXP THEN DECL-SEQUENCIA END\n" +
                                         "REPEAT-STATMENT -> REPEAT BEGIN DECL-SEQUENCIA END UNTIL EXP\n" +
                                         "WHILE-STATMENT -> WHILE EXP DO BEGIN DECL-SEQUENCIA END\n" +
                                         "ATRIB-STATMENT -> ide := EXP\n" +
                                         "READ-STATMENT -> READ ( ide )\n" +
                                         "WRITE-STATMENT -> WRITE ( EXP )\n" +
                                         "EXP -> EXP-SIMPLES X\n" +
                                         "X -> COMP-OP EXP-SIMPLES | ε\n" +
                                         "COMP-OP -> < | > | = | != | >= | <=\n" +
                                         "EXP-SIMPLES -> TERMO EXP-SIMPLES'\n" +
                                         "EXP-SIMPLES'-> SOMA TERMO EXP-SIMPLES' | ε\n" +
                                         "SOMA -> + | -\n" +
                                         "TERMO -> FATOR TERMO'\n" +
                                         "TERMO'-> MULT FATOR TERMO' | ε\n" +
                                         "MULT -> * | /\n" +
                                         "FATOR -> ( EXP ) | num | ide\n";
    }
}
