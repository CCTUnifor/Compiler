import sys
import io
from Entidades.Grammar import Grammar
from Services.GrammarTableService import TableService

"""
"""
file_name = "arquivos/Gramática Tiny.txt"
file_name = "arquivos/gramaticas/Gramática ETF.txt"
file_name = "arquivos/gramaticas/Gramática SXYZ.txt"
file_name = "arquivos/gramaticas/Gramática EB.txt"
file_name = "arquivos/gramaticas/Gramática EABCD.txt"

if(len(sys.argv) > 1):
    file_name = str(sys.argv[1])

file_obj = io.open(file_name, "r", encoding='utf8')

fileTxt = file_obj.read()
g = Grammar.Grammar(fileTxt)

compileGrammarService = TableService(g)
compileGrammarService.compile()

print('-----------------------GRAMÁTICA-----------------------')
print(g)

print('-------------------------FIRST-------------------------')
for term in g.Terms:
    print(term.strFirst())

print('\n------------------------FOLLOW-------------------------')
for term in g.Terms:
    print(term.strFollow())

file_obj.close()