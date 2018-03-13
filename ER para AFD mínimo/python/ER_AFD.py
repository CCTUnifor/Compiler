from Lexical import LexicalAnalyzer as LxA
from Sintatic import SintaticAnalyzer as StA
import ThompsonPrinter as printer

# http://matt.might.net/articles/parsing-regex-with-recursive-descent/
# install Matplotlib
# install networkx

if( __name__ == "__main__"):
    # entry = '(e|d)+'
    entry = 'a(e|d)*'
    # entry = '78d2 abc d 527000'
    # entry = '(a|b)|c'
    # entry= '(|c)*' # erro
    tokens = LxA.analyze(entry)
    for i in tokens:
        print(i)

    sintaticAnalyzer = StA(tokens)
    graph = sintaticAnalyzer.analyze()

    printer.printTable(graph)    
    printer.printMatplotlib(graph)
    