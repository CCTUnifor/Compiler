import sys
from Lexical import LexicalAnalyzer as LxA
from Sintatic import SintaticAnalyzer as StA
from subsetsBuilder import Builder as SBuilder
import ThompsonPrinter as printer

# http://matt.might.net/articles/parsing-regex-with-recursive-descent/
# install Matplotlib
# install networkx

if( __name__ == "__main__"):
    file_name = "teste.txt"

    if(len(sys.argv) > 1):
        file_name = str(sys.argv[1])

    with open(file_name, "r") as file_obj:
        line = file_obj.readline()
        while (line):
            entry = line.strip()
            
            tokens = LxA.analyze(entry)

            sintaticAnalyzer = StA(tokens)
            graph = sintaticAnalyzer.analyze()

            sb = SBuilder(graph)
            matrix = sb.build()

            printer.printMinimunMatrix(matrix)

            line = file_obj.readline()

    file_obj.close()
        

    
    # entry = '(e|d)+'
    # entry = '78d2 abc d 527000'
    # entry = 'aaa*a'
    # entry = 'e(e|d)*'
    # entry = 'e(e|d)*'
    # entry = '(ba|bb)*|(ab|aa)*'
    # entry = '(a|b)|c'
    # entry = '(|c)*' # erro
    # for i in tokens:
    #     print(i)

    # printer.printTable(graph)    
    # printer.printMatplotlib(graph)