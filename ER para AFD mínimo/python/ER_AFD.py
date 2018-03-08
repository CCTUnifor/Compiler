from Lexical import LexicalAnalyzer as LxA
from Sintatic import SintaticAnalyzer as StA

entry= 'e(e|d)*' # falta implementar os ()
# entry = 'e|d'
# entry = 'e*'
# entry = '78d2'


tokens = LxA.analyze(entry)
StA.analyze(tokens)


# root = ERtoAFNE().convert(tokens)
# matrix = AFNEtoAFD().convert(root)

# print(tokens)