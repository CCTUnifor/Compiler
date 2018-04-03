import sys
import io
from Entidades.Grammar import Grammar
from Services.GrammarTableService import TableService
import printer

input_file_directory = "arquivos/inputs/input "
grammar_file_directory = "arquivos/gramaticas/Gramática "

grammar_name = "EABCD"
grammar_name = "SXYZ"
grammar_name = "EB"
grammar_name = "ETF"
grammar_name = "Tiny2"
grammar_name = "Tiny"

grammar_file_name = grammar_file_directory + grammar_name
input_file_name = input_file_directory + grammar_name

if(len(sys.argv) > 2):
    grammar_name = str(sys.argv[1])
    input_name = str(sys.argv[2])

with io.open(grammar_file_name, "r", encoding='utf8') as file_obj:
    fileTxt = file_obj.read()
    g = Grammar.Grammar(fileTxt)

with io.open(input_file_name, "r", encoding='utf8') as file_obj:
    fileTxt = file_obj.read()

    compileGrammarService = TableService(g)
    tokens, historic = compileGrammarService.compile(fileTxt)

    # printer.Grammar_Printer(g)
    # printer.Grammar_Table_Printer(compileGrammarService)
    # printer.LexicPrint(tokens)
    # printer.CompileHistoric(historic)


