from Lexical import LexicalAnalyzer as LxA
from Sintatic import SintaticAnalyzer as StA

# http://matt.might.net/articles/parsing-regex-with-recursive-descent/
# install Matplotlib
# install networkx

if( __name__ == "__main__"):
    # entry = '(e|d)+'
    # entry = 'e*'
    # entry = '78d2 abc d 527000'
    entry = 'a|b'
    # entry= '(|c)*'
    tokens = LxA.analyze(entry)
    for i in tokens:
        print(i)

    sintaticAnalyzer = StA(tokens)
    graph = sintaticAnalyzer.analyze()
    print('------------------MATRIZ-----------------')
    graph.printTable()
    # print(str(graph.root))
    print('------------------GRAFO------------------')
    graph.printMatplotlib()

    # root = ERtoAFNE().convert(tokens)
    # matrix = AFNEtoAFD().convert(root)

    # print(tokens)
