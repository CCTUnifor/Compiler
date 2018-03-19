import sys
from Lexical import LexicalAnalyzer as LxA
from Sintatic import RecursiveDescendentSintaticAnalyzer as RDSintaticA

# http://matt.might.net/articles/parsing-regex-with-recursive-descent/
# install Matplotlib
# install networkx

if( __name__ == "__main__"):
    file_name = "arquivos/teste.txt"

    if(len(sys.argv) > 1):
        file_name = str(sys.argv[1])

    with open(file_name, "r") as file_obj:
        line = file_obj.read()

        lexic = LxA(line)

        print(lexic.getToken())
        # while(lexic.notEnded()):
            
            

        # SA = RDSintaticA(lexic)
        # SA.analyze()



    file_obj.close()