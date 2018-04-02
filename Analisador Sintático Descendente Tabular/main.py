import sys
import io
from Entidades.Grammar import Grammar
from Services.GrammarTableService import TableService
import printer

"""
"""
file_name = "arquivos/Gramática Tiny.txt"
file_name = "arquivos/gramaticas/Gramática EABCD.txt"
file_name = "arquivos/gramaticas/Gramática EB.txt"
file_name = "arquivos/gramaticas/Gramática SXYZ.txt"
file_name = "arquivos/gramaticas/Gramática ETF.txt"

if(len(sys.argv) > 1):
    file_name = str(sys.argv[1])

file_obj = io.open(file_name, "r", encoding='utf8')

fileTxt = file_obj.read()
g = Grammar.Grammar(fileTxt)

compileGrammarService = TableService(g)
compileGrammarService.compile()


printer.Grammar_Printer(g)
printer.Grammar_Table_Printer(compileGrammarService)


file_obj.close()