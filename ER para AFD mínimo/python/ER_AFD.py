from Lexical import LexicalAnalyzer as LxA
from Sintatic import SintaticAnalyzer as StA


if( __name__ == "__main__"):
    # entry = '(e|d)+'
    # entry = 'e*'
    # entry = '78d2'
    entry= '(|c)*'
    # http://matt.might.net/articles/parsing-regex-with-recursive-descent/
    tokens = LxA.analyze(entry)
    for i in tokens:
        print(i)

    sintaticAnalyzer = StA(tokens)
    sintaticAnalyzer.analyze()

    # root = ERtoAFNE().convert(tokens)
    # matrix = AFNEtoAFD().convert(root)

    # print(tokens)
