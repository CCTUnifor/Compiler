import sys
import io

from Core.Entities.Grammar import Grammar
from Core.Services.TableDescendentSintaticAnalyzer import TableService

import printer


input_file_directory = "misc/inputs/input "
grammar_file_directory = "misc/grammars/Gramática "

grammar_name = "EB"
grammar_name = "Tiny2"
grammar_name = "EABCD"
grammar_name = "SXYZ"
grammar_name = "SAB"
grammar_name = "ETF"
grammar_name = "Tiny"

grammar_file_name = grammar_file_directory + grammar_name
input_file_name = input_file_directory + grammar_name

if(len(sys.argv) > 2):
    grammar_name = str(sys.argv[1])
    input_name = str(sys.argv[2])

with io.open(grammar_file_name, "r", encoding='utf8') as file_obj:
    fileTxt = file_obj.read()
    g = Grammar(fileTxt)

compileGrammarService = TableService(g)
compileGrammarService.compileGrammar()

with io.open(input_file_name, "r", encoding='utf8') as file_obj:
    fileTxt = file_obj.read()

    tokens, historic = compileGrammarService.compile(fileTxt)


printer.Grammar_Printer(g)
printer.Grammar_Table_Printer(compileGrammarService)
printer.LexicPrint(tokens)
printer.CompileHistoric(historic)

