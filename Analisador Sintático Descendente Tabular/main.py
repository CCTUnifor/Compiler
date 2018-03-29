import sys
from Grammar import Grammar

# http://matt.might.net/articles/parsing-regex-with-recursive-descent/
# install Matplotlib
# install networkx

# if( __name__ == "__main__"):
file_name = "arquivos/Gramática Final.txt"

if(len(sys.argv) > 1):
    file_name = str(sys.argv[1])

file_obj = open(file_name, "r")

fileTxt = file_obj.read()
g = Grammar(fileTxt)

# print(g.matched)
# print(g.NonTerminals)
# print(g.Alphabet)
# print(g.StartSimbol)
# print(g.Premises)

g.makeTerms()
for term in g.getTerms():
    print(term)
    
    

    # SA = RDSintaticA(lexic)
    # SA.parse()



file_obj.close()