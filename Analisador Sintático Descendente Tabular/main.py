import sys
import io
from Entidades.Grammar import Grammar

"""
file_name = "arquivos/Gramática Tiny.txt"
file_name = "arquivos/gramaticas/Gramática EABCD.txt"
file_name = "arquivos/gramaticas/Gramática EB.txt"
file_name = "arquivos/gramaticas/Gramática SXYZ.txt"
"""
file_name = "arquivos/gramaticas/Gramática ETF.txt"

if(len(sys.argv) > 1):
    file_name = str(sys.argv[1])

file_obj = io.open(file_name, "r", encoding='utf8')

fileTxt = file_obj.read()
g = Grammar.Grammar(fileTxt)

print('-----------------------GRAMÁTICA-----------------------')
print(g)

# g.build_grammar_routine()

print('-------------------------FIRST-------------------------')
for term in g.Terms:
    print(term.strFirst())

print('\n------------------------FOLLOW-------------------------')
for term in g.Terms:
    print(term.strFollow())

file_obj.close()