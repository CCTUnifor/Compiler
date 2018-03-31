import sys
import Grammar

# file_name = "arquivos/Gramática Tiny.txt"
# file_name = "arquivos/gramaticas/Gramática ETF.txt"
# file_name = "arquivos/gramaticas/Gramática SXYZ.txt"
# file_name = "arquivos/gramaticas/Gramática EABCD.txt"
file_name = "arquivos/gramaticas/Gramática EB.txt"

if(len(sys.argv) > 1):
    file_name = str(sys.argv[1])

file_obj = open(file_name, "r")

fileTxt = file_obj.read()
g = Grammar.Grammar(fileTxt)

# print(g.matched)
# print(g.NonTerminals)
# print(g.Alphabet)
# print(g.StartSimbol)
# print(g.Premises)

g.build_grammar_matrix()
for term in g.get_terms():
    # print('-------------------------'+term.left+'-------------------------')
    # print("Term:"+term)
    print(term.strFirst())
    

file_obj.close()