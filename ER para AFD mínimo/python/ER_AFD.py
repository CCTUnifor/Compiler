from Lexical import LexicalAnalyzer as LxA
from Sintatic import SintaticAnalyzer as StA


if( __name__ == "__main__"):
    # entry = 'e|d'
    # entry = 'e*'
    # entry = '78d2'
    # http://matt.might.net/articles/parsing-regex-with-recursive-descent/
    entry= 'e(e|d)*'

    tokens = LxA.analyze(entry)

    sintaticAnalyzer = StA(tokens)
    sintaticAnalyzer.analyze()

    # root = ERtoAFNE().convert(tokens)
    # matrix = AFNEtoAFD().convert(root)

    # print(tokens)
